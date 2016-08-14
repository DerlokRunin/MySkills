using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


/**
 *  Класс управляющий приложением
 *  
 *  @autor Derlok Runin
 *  @version 1.0
 */

public class ScreenHolder : MonoBehaviour {

    public TaskList taskList;

    //добавление задачи
    public InputField inputAddTextTask;

    //редактирование задачи
    public InputField inputEditIdTask;
    public InputField inputEditTextTask;

    //кнопки удаления и редактирования задачи
    public Button butEdit;
    public Button butDelete;


    void Start()
    {
        //Создаем лист задач
        taskList.CreateTaskList();
        SelectedTask();


    }

    /// <summary>
    /// Кнопка создание задачи
    /// </summary>
    public void CreateTask()
    {
        string text = inputAddTextTask.text;
        taskList.CreateTask(text);
        SelectedTask();
    }

    /// <summary>
    /// Кнопка редактирования задачи
    /// </summary>
    public void EditTask()
    {
        int id = Convert.ToInt32(inputEditIdTask.text);
        string text = inputEditTextTask.text;
        taskList.EditTask(id, text);
    }

    /// <summary>
    /// Возможность выбрать задачу
    /// </summary>
    public void SelectedTask()
    {
        //Появление кнопок редактирования и удаления
        foreach (Task task in taskList.tasks)
        {
            Toggle toggle = task.toggle;
            int id = task.id;
            Text label = task.label;
            toggle.onValueChanged.AddListener((on) => {
                if (on)
                {
                    butEdit.gameObject.SetActive(true);
                    butDelete.gameObject.SetActive(true);
                    butEdit.onClick.AddListener(() =>
                    {
                        inputEditIdTask.text = id + "";
                        inputEditTextTask.text = label.text;
                    });
                    butDelete.onClick.AddListener(() =>
                    {
                        taskList.DeleteTask(id);
                        SelectedTask();
                    });
                }
                else
                {
                    butEdit.gameObject.SetActive(false);
                    butDelete.gameObject.SetActive(false);
                }
            });
        }
    }
}
