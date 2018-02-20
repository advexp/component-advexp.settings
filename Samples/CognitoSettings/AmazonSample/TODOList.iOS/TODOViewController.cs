using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using MonoTouch.Dialog;
using TODOListPortableLibrary;

namespace TODOList.iOS
{
    public class TODOViewController : DialogViewController
    {
        List<Task> todoLists;
        Task currentTask;
        TaskDialog taskDialog;
        DialogViewController detailsScreen;
        BindingContext context;

        public TODOViewController()
            : base(UITableViewStyle.Plain, null)
        {
            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add), false);
            NavigationItem.RightBarButtonItem.Clicked += (sender, e) =>
            {
                taskDialog = new TaskDialog()
                {
                    Id = Guid.NewGuid().ToString()
                };
                context = new BindingContext(this, taskDialog, "New Task");
                detailsScreen = new DialogViewController(context.Root, true);
                ActivateController(detailsScreen);
            };

            this.RefreshRequested += delegate {
                NSTimer.CreateScheduledTimer(1, delegate {
                    CognitoSyncUtils.Synchronize();
                    this.ReloadComplete();
                });
            };

            CognitoSyncUtils.SetSyncAction((exception) =>
            {
                if (exception != null)
                {
                    Console.WriteLine("ERROR: " + exception.Message);
                    return;
                }
                CognitoSyncUtils.LoadTasks();
                todoLists = CognitoSyncUtils.GetTasks();
                if (todoLists != null)
                {
                    InvokeOnMainThread(() =>
                    {
                        var e = from t in todoLists
                                select (Element)new CheckboxElement((string.IsNullOrEmpty(t.Title) ? string.Empty : t.Title), t.Completed);
                        var section = new Section();
                        foreach (var element in e)
                        {
                            section.Add(element);
                        }

                        Root = new RootElement("Todo List");
                        Root.Add(section);
                    });
                }
            });
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CognitoSyncUtils.Synchronize();
        }

        public override void Selected(NSIndexPath indexPath)
        {
            base.Selected(indexPath);
            var row = indexPath.Row;
            var task = todoLists[row];
            ShowDetails(task);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        private void ShowDetails(Task task)
        {
            currentTask = task;
            taskDialog = new TaskDialog(task);
            context = new BindingContext(this, taskDialog, taskDialog.Name);
            detailsScreen = new DialogViewController(context.Root, true);
            ActivateController(detailsScreen);
        }

        public void SaveTask()
        {
            context.Fetch();
            currentTask = new Task();
            currentTask.Id = taskDialog.Id;
            currentTask.Title = taskDialog.Name;
            currentTask.Description = taskDialog.Description;
            currentTask.Completed = taskDialog.Completed;
            NavigationController.PopViewController(true);
            CognitoSyncUtils.SaveTask(currentTask);
            currentTask = null;
        }

        public void DeleteTask()
        {
            if (currentTask != null)
            {
				context.Fetch();
				NavigationController.PopViewController(true);
                CognitoSyncUtils.DeleteTask(taskDialog.Id);
				currentTask = null;
            }
        }
    }

    public class TaskDialog
    {
        public TaskDialog()
        {
        }

        public TaskDialog(Task task)
        {
            Id = task.Id;
            Name = task.Title;
            Description = task.Description;
            Completed = task.Completed;
        }

        [Skip]
        public string Id { get; set; }

        [Entry("task title")]
        public string Name { get; set; }

        [Entry("description")]
        public string Description { get; set; }

        [Entry("Completed")]
        public bool Completed { get; set; }

        [Section("")]
        [OnTap("SaveTask")]
        [Alignment(UITextAlignment.Center)]
        public string Save;

        [OnTap("DeleteTask")]
        [Alignment(UITextAlignment.Center)]
        public string Delete;
    }
}