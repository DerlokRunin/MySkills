using UnityEngine;
using System.IO;
using System.Text;
using System.Data;
using Mono.Data.SqliteClient;
using System;
using System.Collections.Generic;
/**
* Скрипт для работы с бд sqlite
*
db = new SQLiteManager();
*
* Query - обычный sql запрос, не возращающий результат
db.Query("SELECT rowid, text FROM tasks WHERE rowid = 1");
*
* QueryResult - sql на вывод результата. Возращает коллекцию LIst<string[]>
List<string[]> result = db.QueryResult("SELECT rowid, text FROM tasks");
foreach (var item in result)
{
  Debug.Log(item[1]);
}
* 
*/

namespace SQLiteDB
{
    public class SQLiteManager
    {
        public static SQLiteManager instance = null;
        public bool debugMode = true;

        private const string DB_NAME = "Skills";
        /*private static readonly string DB_LOCATION = "URI=file:"
           + Application.dataPath + Path.DirectorySeparatorChar
           + "Plugins" + Path.DirectorySeparatorChar
           + "SQLiter" + Path.DirectorySeparatorChar
           + "Databases" + Path.DirectorySeparatorChar
           + DB_NAME + ".db";*/
        private static string DB_LOCATION = "";

        /// <summary>
        /// Обьекты бд
        /// </summary>
        private IDbConnection connection = null;
        private IDbCommand command = null;
        private IDataReader reader = null;

        public SQLiteManager()
        {
            if (instance == null)
                instance = this;
            //Настраиваем расположение бд для разных платформ
            if (DB_LOCATION == "")
            {
                
#if UNITY_EDITOR
                var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DB_NAME);
                dbPath += ".db";
#else
                var DatabaseName = DB_NAME + ".db";
                var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

            if (!File.Exists(filepath))
            {
                //Debug.Log("Database not in Persistent path");
                // if it doesn't ->
                // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
                
                var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                // then save to Application.persistentDataPath
                File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
           
#elif UNITY_WINRT
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#endif

                //Debug.Log("Database written");
            }

            var dbPath = filepath;
#endif
                Debug.Log("Final PATH: " + dbPath);
                DB_LOCATION = "URI=file:" + dbPath;
            }
        }

        /// <summary>
        /// Просто запрос к бд без результата
        /// INSERT OR REPLACE INTO таблица () VALUES (); 
        /// DELETE FROM таблица WHERE ....
        /// UPDATE OR REPLACE таблица SET поле = значение WHERE....
        /// </summary>
		/// <returns>id затронутой строки</returns>
        public int Query(string text)
        {
            this.Open();
            command.CommandText = text;
            command.ExecuteNonQuery();

			command.CommandText = "SELECT last_insert_rowid() AS RowId";
			int rowId = Convert.ToInt32(command.ExecuteScalar());

            this.Close();
			return rowId;
        }

        /// <summary>
        /// Запрос к бд с результатом без *
        /// </summary>
        /// <returns>Коллекция строк</returns>
        public List<string[]> QueryResult(string text)
        {
            this.Open();
            List<string[]> data = new List<string[]>();
            command.CommandText = text;
            reader = command.ExecuteReader();

            //Количество данных
            int num = text.Split(new string[] { "," }, StringSplitOptions.None).Length;
            while (reader.Read())
            {
                string[] arr = new string[num];
                for (int i = 0; i < num; i++)
                {
                    arr[i] = reader.GetString(i);
                    
                }
                data.Add(arr);
            }
            this.Close();
            return data;
        }

        /// <summary>
        /// Открыть соединение с бд
        /// </summary>
        private void Open()
        {
            connection = (IDbConnection)new SqliteConnection(DB_LOCATION);
            connection.Open();//Открываем соединение
            command = connection.CreateCommand();

            //Настройки бд
            command.CommandText = "PRAGMA journal_mode = WAL";
            command.ExecuteNonQuery();
            command.CommandText = "PRAGMA synchronous = OFF";
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Закрытие соединения с бд
        /// </summary>
        private void Close()
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
            reader = null;

            if (command != null)
                command.Dispose();
            command = null;

            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
            connection = null;
        }

    }
}