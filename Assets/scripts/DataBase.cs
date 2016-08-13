using UnityEngine;
using SQLiteDB;
using System;
using System.Collections.Generic;
/**
 *  Интерфейс для взаимодействия с базой данных
 *  SQlite
 *  
 *  @autor Derlok Runin
 *  @version 1.0
 */

public class DataBase : MonoBehaviour {

    private static SQLiteManager db;


    void Awake()
    {
        db = new SQLiteManager();
    }

    /// <summary>
    /// Запрос к бд без данных
    /// </summary>
    /// <param name="sql">текст запроса</param>
    /// <returns>id затронутой строки</returns>
    public static int query(string sql)
    {
        return db.Query(sql);
    }


    /// <summary>
    /// Запрос к бд с результатом без *(нужно задавать все параметры)
    /// </summary>
    /// <param name="sql">текст запроса</param>
    /// <returns>Коллекция строк</returns>
    public static List<string[]> queryResult(string sql)
    {
        return db.QueryResult(sql);
    }

}