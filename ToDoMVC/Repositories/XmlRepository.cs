using ToDoMVC.Models;
using ToDoMVC.Models.ViewModels;
using System.Xml.Linq;

namespace ToDoMVC.Repositories
{
    public class XmlRepository : IRepository
    {
        private static string tasksPath = "XML/Tasks.xml";
        private static string categoriesPath = "XML/Categories.xml";

        private static Todo CreateTodo(XElement elem)
        {
            //id
            int todoId = int.Parse(elem.Attribute("Id").Value);
            //name 
            string name = elem.Element("Name").Value;
            //deadline
            DateTime? deadline = new DateTime();
            if (elem.Element("Deadline").Value == "")
                deadline = null;
            else
                deadline = DateTime.Parse(elem.Element("Deadline").Value);
            //is done
            bool isDone = Convert.ToBoolean(elem.Element("IsDone").Value);
            //categoryid
            int? categoryId;
            if (elem.Element("CategoryId").Value == "")
                categoryId = null;
            else
                categoryId = int.Parse(elem.Element("CategoryId").Value);
            //categoryname
            string? categoryName = null;
            if (categoryId != null)
            {
                var doc = XDocument.Load(categoriesPath);
                var elements = doc.Root.Descendants("Category");
                foreach (var e in elements)
                    if (e is not null && e.Attribute("Id").Value == categoryId.ToString())
                        categoryName = e.Element("Name").Value;
            }
            return new Todo(todoId, name, deadline, isDone, categoryId, categoryName);
        }

        private static Category CreateCategory(XElement elem)
        {
            //id
            int todoId = int.Parse(elem.Attribute("Id").Value);
            //name 
            string name = elem.Element("Name").Value;
            return new Category(todoId, name);
        }

        public static List<Todo> GetAllTodos(int? categoryid = null)
        {
            var doc = XDocument.Load(tasksPath);
            List<Todo> tasks = new();
            IEnumerable<XElement> elements = doc.Root.Descendants("Task");
            foreach (var elem in elements)
            {
                if (elem is not null)
                {
                    var todo = CreateTodo(elem);
                    if (categoryid == null)
                    {
                        tasks.Add(todo);
                    }
                    else if (categoryid == todo.CategoryId)
                    {
                        tasks.Add(todo);
                    }
                }
            }
            return tasks;
        }

        public static List<Category> GetAllCategories()
        {
            var doc = XDocument.Load(categoriesPath);
            List<Category> categories = new();
            IEnumerable<XElement> elements = doc.Root.Descendants("Category");
            foreach (var elem in elements)
            {
                if (elem is not null)
                {
                    categories.Add(CreateCategory(elem));
                }
            }
            return categories;
        }

        List<Todo> IRepository.GetPageOfTodos(int page, int pageSize)
        {
            var doc = XDocument.Load(tasksPath);
            IEnumerable<XElement> tasksXElements = doc.Root.Descendants("Task");
            //sort!
            List<Todo> tasks = new();
            int i = 0;
            foreach (var elem in tasksXElements)
            {
                if (elem is not null && i >= pageSize * (page - 1) && i < pageSize * page)
                {
                    tasks.Add(CreateTodo(elem));
                }
                i++;
            }
            return tasks;
        }

        PagingInfo IRepository.GetTodosPagingInfo(int page, int pageSize)
        {
            if (page < 1) page = 1;
            return new PagingInfo(page, pageSize, XDocument.Load(tasksPath).Root.Descendants("Task").Count());
        }

        Todo? IRepository.GetTodoById(int id)
        {
            var doc = XDocument.Load(tasksPath);
            var elements = doc.Root.Descendants("Task");
            foreach (var elem in elements)
            {
                if (elem is not null && elem.Attribute("Id").Value == id.ToString())
                {
                    return CreateTodo(elem);
                }
            }
            return null;
        }

        Todo? IRepository.GetLastTodo()
        {
            var doc = XDocument.Load(tasksPath);
            var elements = doc.Root.Descendants("Task");
            int lastId = 0;
            XElement? last = null;
            foreach (var elem in elements)
            {
                if (elem is not null && Convert.ToInt32(elem.Attribute("Id").Value) >= lastId)
                {
                    last = elem;
                    lastId = Convert.ToInt32(elem.Attribute("Id").Value);
                }
            }
            if (last != null)
                return CreateTodo(last);
            else
                return null;
        }

        int IRepository.CreateTodo(IFormCollection collection)
        {
            //name
            string name = Convert.ToString(collection["Name"]);
            //deadline
            bool x = DateTime.TryParse(collection["Deadline"], out DateTime deadline1);
            DateTime? deadline = x ? deadline1 : null;
            //is done
            bool isdone = Convert.ToBoolean(collection["IsDone"].ToString().Split(',')[0]);
            //category
            int? categoryId;
            if (collection["CategoryName"] == "")
                categoryId = null;
            else categoryId = Convert.ToInt32(collection["CategoryName"]);

            var lastTodo = ((IRepository)this).GetLastTodo();
            int lastId = 0;
            if (lastTodo != null) lastId = lastTodo.Id;
            var doc = XDocument.Load(tasksPath);
            var newTask = new XElement("Task",
                new XAttribute("Id", lastId + 1),
                new XElement("Name", name),
                new XElement("Deadline", deadline),
                new XElement("IsDone", isdone),
                new XElement("CategoryId", categoryId)
                );
            doc.Root.Add(newTask);
            doc.Save(tasksPath);
            return 1;
        }
        
        int IRepository.UpdateTodo(int id, IFormCollection collection)
        {
            int updated = 0;
            //name
            string name = Convert.ToString(collection["Name"]);
            //deadline
            bool x = DateTime.TryParse(collection["Deadline"], out DateTime deadline1);
            DateTime? deadline = x ? deadline1 : null;
            //is done
            bool isdone = Convert.ToBoolean(collection["IsDone"].ToString().Split(',')[0]);
            //category
            int? categoryId;
            if (collection["CategoryName"] == "")
                categoryId = null;
            else categoryId = Convert.ToInt32(collection["CategoryName"]);

            var doc = XDocument.Load(tasksPath);
            foreach (var el in doc.Root.Elements())
            {
                if (el.Attribute("Id").Value == id.ToString())
                {
                    el.Element("Name").Value = name;
                    el.Element("Deadline").Value = deadline == null ? "" : deadline.ToString();
                    el.Element("IsDone").Value = isdone.ToString();
                    el.Element("CategoryId").Value = categoryId == null ? "" : categoryId.ToString();
                    updated = 1;
                }
            }
            doc.Save(tasksPath);
            return updated;
        }

        int IRepository.DeleteTodo(int id, IFormCollection collection)
        {
            var doc = XDocument.Load(tasksPath);

            foreach (var elem in doc.Root.Elements())
            {
                if (elem.Attribute("Id").Value == id.ToString())
                {
                    elem.Remove();
                }
            }

            doc.Save(tasksPath);

            return 1;
        }

        bool IRepository.TodoExists(int id)
        {
            var doc = XDocument.Load(tasksPath);
            var elements = doc.Root.Descendants("Task");
            foreach (var elem in elements)
            {
                if (elem is not null && elem.Attribute("Id").Value == id.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        
        List<Category> IRepository.GetPageOfCategories(int page, int pageSize)
        {
            var doc = XDocument.Load(categoriesPath);
            IEnumerable<XElement> categoriesXElements = doc.Root.Descendants("Category");
            List<Category> categories = new();
            int i = 0;
            foreach (var elem in categoriesXElements)
            {
                if (elem is not null && i >= pageSize * (page - 1) && i < pageSize * page)
                {
                    categories.Add(CreateCategory(elem));
                }
                i++;
            }
            return categories;
        }

        PagingInfo IRepository.GetCategoriesPagingInfo(int page, int pageSize)
        {
            if (page < 1) page = 1;
            return new PagingInfo(page, pageSize, XDocument.Load(categoriesPath).Root.Descendants("Category").Count());
        }
        
        Category? IRepository.GetCategoryById(int id)
        {
            var doc = XDocument.Load(categoriesPath);
            var elements = doc.Root.Descendants("Category");
            foreach (var elem in elements)
            {
                if (elem is not null && elem.Attribute("Id").Value == id.ToString())
                {
                    return CreateCategory(elem);
                }
            }
            return null;
        }

        Category? IRepository.GetLastCategory()
        {
            var doc = XDocument.Load(categoriesPath);
            var elements = doc.Root.Descendants("Category");
            int lastId = 0;
            XElement? last = null;
            foreach (var elem in elements)
            {
                if (elem is not null && Convert.ToInt32(elem.Attribute("Id").Value) >= lastId)
                {
                    last = elem;
                    lastId = Convert.ToInt32(elem.Attribute("Id").Value);
                }
            }
            if (last != null)
                return CreateCategory(last);
            else
                return null;
        }
        
        int IRepository.CreateCategory(IFormCollection collection)
        {
            //name
            string name = Convert.ToString(collection["Name"]);

            var lastCategory = ((IRepository)this).GetLastCategory();
            int lastId = 0;
            if (lastCategory != null) lastId = lastCategory.Id;
            var doc = XDocument.Load(categoriesPath);
            var newCategory = new XElement("Category",
                new XAttribute("Id", lastId + 1),
                new XElement("Name", name)
                );
            doc.Root.Add(newCategory);
            doc.Save(categoriesPath);
            return 1;
        }
        //nd
        int IRepository.UpdateCategory(int id, IFormCollection collection)
        {
            int updated = 0;
            string name = Convert.ToString(collection["Name"]);

            var doc = XDocument.Load(tasksPath);
            foreach (var el in doc.Root.Elements())
            {
                if (el.Attribute("Id").Value == id.ToString())
                {
                    el.Element("Name").Value = name;
                    updated = 1;
                }
            }
            doc.Save(tasksPath);
            return updated;
        }
        
        int IRepository.DeleteCategory(int id, IFormCollection collection)
        {
            var doc = XDocument.Load(categoriesPath);
            int isDeleted = 0;
            foreach (var elem in doc.Root.Elements())
            {
                if (elem.Attribute("Id").Value == id.ToString())
                {
                    var doc2 = XDocument.Load(tasksPath);
                    IEnumerable<XElement> tasksXElements = doc.Root.Descendants("Task");
                    isDeleted = 1;
                    foreach (var elem2 in tasksXElements)
                    {
                        if (elem is not null && elem.Element("CategoryId").Value == id.ToString())
                        {
                            isDeleted = 0;
                        }
                    }
                    if (isDeleted == 1)
                    {
                        elem.Remove();
                        return isDeleted;
                    }
                }
            }
            doc.Save(categoriesPath);
            return isDeleted;
        }
        bool IRepository.CategoryExists(int id)
        {
            var doc = XDocument.Load(categoriesPath);
            var elements = doc.Root.Descendants("Category");
            foreach (var elem in elements)
            {
                if (elem is not null && elem.Attribute("Id").Value == id.ToString())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
