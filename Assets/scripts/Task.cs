using UnityEngine;
using UnityEngine.UI;
using System;

/**
 *  Класс описывающий сущность - ЗАДАЧУ
 *  
 *  @autor Derlok Runin
 *  @version 1.0
 */

public class Task : MonoBehaviour {

    public int id;
    public Text label;
    public Toggle toggle;
    public GameObject obj;

    /// <summary>
    /// Создаем новую задачу
    /// </summary>
    /// <param name="obj">обьект на сцене</param>
    /// <param name="text">описание задачи</param>
    public Task(GameObject obj, string text)
    {
        this.obj = obj;
        this.label = obj.transform.FindChild("Label").GetComponent<Text>();
        this.label.text = text;
        this.toggle = obj.GetComponent<Toggle>();

        string formatData = DateTime.Now.ToString("yyyy-dd-MM hh:mm:ss");
        this.id = DataBase.query("INSERT OR REPLACE INTO tasks (text, data) VALUES('" + text + "', '" + formatData + "')");
    }

    /// <summary>
    /// Добавляем новую задачу
    /// </summary>
    /// <param name="obj">обьект на сцене</param>
    /// <param name="text">описание задачи</param>
    public Task(GameObject obj, string text, int id)
    {
        this.obj = obj;
        this.label = obj.transform.FindChild("Label").GetComponent<Text>();
        this.label.text = text;
        this.toggle = obj.GetComponent<Toggle>();
        this.id = id;
    }

    /// <summary>
    /// Редактирование задачи на сцене и в бд
    /// </summary>
    /// <param name="text">описание задачи</param>
    public void Edit(string text)
    {
        DataBase.query("UPDATE tasks SET text = '" + text + "' WHERE rowid = '" + this.id + "'");
        this.label.text = text;
    }

    /// <summary>
    /// Удалить задачу со сцены и в бд
    /// </summary>
    public void Delete()
    {
        DataBase.query("DELETE FROM tasks WHERE rowid = '" + this.id + "'");
        Destroy(obj);
    }
}
