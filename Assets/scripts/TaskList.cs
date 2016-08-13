using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


/**
 *  Класс описывающий сущность - ЛИСТ ЗАДАЧ
 *  
 *  @autor Derlok Runin
 *  @version 1.0
 */

public class TaskList : MonoBehaviour
{

    public GameObject doing;

    private List<Task> tasks = new List<Task>();

    //Размеры
    private int height = 37;
    private int currentPosition = 0;

    /// <summary>
    ///  Создание листа с задачами 
    /// </summary>
    public void CreateTaskList()
    {
        List<string[]> result = DataBase.queryResult("SELECT rowid, text, data FROM tasks");
        foreach (var item in result)
        {
            //Размещаем на сцене
            GameObject obj = InstansTask();

            //Сохраняем задачу и добавляем в лист
            Task task = new Task(obj, item[1], Convert.ToInt32(item[0]));
            tasks.Add(task);
        }
    }

    /// <summary>
    /// Очистить лист с задачами
    /// </summary>
    public void ClearTaskList()
    {
        tasks.Clear();
    }

    /// <summary>
    /// Создание задачи
    /// </summary>
    /// <param name="text">текст задачи</param>
    public void CreateTask(string text)
    {
        //Размещаем на сцене
        GameObject obj = InstansTask();

        //Сохраняем задачу и добавляем в лист
        Task task = new Task(obj, text);
        tasks.Add(task);
    }

    /// <summary>
    /// Удаление задачи
    /// </summary>
    /// <param name="id">id задачи</param>
    public void DeleteTask(int id)
    {
        foreach (Task task in tasks)
        {
            if(task.id == id)
            {
                task.Delete();
                tasks.Remove(task);
            }
        }
    }

    /// <summary>
    /// Редактирование задачи
    /// </summary>
    /// <param name="id">id задачи</param>
    /// <param name="text">новый текст</param>
    public void EditTask(int id, string text)
    {
        foreach (Task task in tasks)
        {
            if (task.id == id)
            {
                task.Edit(text);
            }
        }
    }

    /// <summary>
    /// Размещаем задачу на сцене
    /// </summary>
    /// <returns>обьект задачи на сцене</returns>
    private GameObject InstansTask()
    {
        GameObject obj = Instantiate(doing) as GameObject;
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(0, currentPosition, 0);
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(-28, height);

        //Увеличиваем лист с задачами
        currentPosition -= height;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (currentPosition * -1) + 20);

        return obj;
    }
}