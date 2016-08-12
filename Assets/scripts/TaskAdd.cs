using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SQLiteDB;
using System;
using System.Collections.Generic;

/**
 *  Класс для работы с задачами
 *  
 *  version 1.0
 */
public class  TaskAdd: MonoBehaviour {

    public GameObject rename;
    public GameObject remove;
    public GameObject doing;
	public GameObject list;
    public InputField inputText;

    public List<GameObject> tasks = new List<GameObject>();

    //Размеры
    private int height = 37;
    private int currentPosition = 0;

    private Button butRename;
    private Button butRemove;

    private SQLiteManager db;

    public void Start()
    {
        db = new SQLiteManager();
        this.getDoings();

        butRemove = remove.GetComponent<Button>();
        //butRename = rename.GetComponent<Button>();

    }

    /// <summary>
    /// Добавить задачу
    /// </summary>
	public void addDoing()
	{
        //записываем задачу в бд
        string formatData = DateTime.Now.ToString("yyyy-dd-MM hh:mm:ss");
        int result = db.Query("INSERT OR REPLACE INTO tasks (text, data) VALUES('" + inputText.text + "', '" + formatData + "')");

		//Размещаем задачи
		Instans(inputText.text, result);

        inputText.text = "";
    }

    /// <summary>
    /// Удалить запись
    /// </summary>
    public void removeDoing(int id, GameObject target)
    {
        //Удаляем запись из бд
        int result = db.Query("DELETE FROM tasks WHERE rowid = '"+ id +"'");

        //Размещаем задачи
        this.removeAllDoing();
        this.getDoings();
    }

    /// <summary>
    /// Удаляем все задачи
    /// </summary>
    public void removeAllDoing()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            Destroy(tasks[i]);
            
        }
        currentPosition = 0;
        tasks.Clear();
    }

    /// <summary>
    /// Достаем с бд все сохраненые задачи
    /// </summary>
    private void getDoings()
    {
        List<string[]> result = db.QueryResult("SELECT rowid, text, data FROM tasks");
        foreach (var item in result)
        {
			Instans(item[1], Convert.ToInt32(item[0]));
        }
    }

    /// <summary>
    /// Добавляет обьект на поле
    /// </summary>
	private void Instans(string text, int id)
    {
        //размещаем
        GameObject obj = Instantiate(doing) as GameObject;
        obj.transform.SetParent(list.transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(0, currentPosition, 0);
        currentPosition -= height;
        //задаем текст
        obj.transform.FindChild("Label").GetComponent<Text>().text = text;
		obj.GetComponent<RectTransform>().sizeDelta = new Vector2(-28, height);
        tasks.Add(obj);
        //Cобытие изменение toggle
        Toggle toggle = obj.GetComponent<Toggle>();
		toggle.onValueChanged.AddListener ((on) => {
            if (on)
            {
                remove.SetActive(true);
                rename.SetActive(true);
                butRemove.onClick.AddListener(() => { removeDoing(id, toggle.gameObject); });
                Debug.Log(id);
            }
            else
            {
                remove.SetActive(false);
                rename.SetActive(false);
            }
		});

        //увеличиваем лист с задачами
        list.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (currentPosition * -1) + 20);
    }
}
