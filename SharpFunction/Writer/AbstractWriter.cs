using System.IO;
using System.Linq;

namespace SharpFunction.Writer
{
    /// <summary>
    ///     Used for inheritance for all other writer classes
    /// </summary>
    public abstract class AbstractWriter
    {
        /// <summary>
        ///     ID of current category
        /// </summary>
        protected string category = "None";

        /// <summary>
        ///     Name of current category
        /// </summary>
        protected string categoryName = "None";

        /// <summary>
        ///     Created categories
        /// </summary>
        protected string[] createdCategories = { };

        /// <summary>
        ///     Writer that will write data to files
        /// </summary>
        protected TextWriter Writer;

        /// <summary>
        ///     Represents main category where everything will be created
        /// </summary>
        public string Main { get; set; }

        /// <summary>
        ///     Returns current category path. Can return null if not specified
        /// </summary>
        public string Category
        {
            get => category;
            set => category = value;
        }

        /// <summary>
        ///     Returns current category name. Can return null if not specified
        /// </summary>
        public string CategoryName
        {
            get => categoryName;
            set => categoryName = value;
        }

        /// <summary>
        ///     Initialize a writer and lock it to functions directory
        /// </summary>
        /// <param name="path">Path to where all the functions are stored</param>
        public void Initialize(string path)
        {
            Main = path;
        }


        /// <summary>
        ///     Creates a category for commands.<br />
        ///     For example: functions in category *name* will be executed as <code>*name*/function</code>
        /// </summary>
        public void CreateCategory(string name)
        {
            category = Path.Combine(Main, name);
            categoryName = name;
            createdCategories.Append(name);
            Directory.CreateDirectory(category);
        }

        /// <summary>
        ///     Changes current category to specified.<br />
        ///     If specified category does not exist, creates it.
        /// </summary>
        /// <param name="name">Name of the category</param>
        public void ChangeCategory(string name)
        {
            if (CategoryExists(name))
            {
                category = Path.Combine(Main, name);
                categoryName = name;
            }
            else
            {
                CreateCategory(name);
            }
        }

        /// <summary>
        ///     Checks whether the category is specified.
        /// </summary>
        /// <returns>True if specified and False if not</returns>
        public bool CategorySpecified()
        {
            if (categoryName != "None" && category != "None") return true;
            return false;
        }

        /// <summary>
        ///     Checks whether the category with specified name exists
        /// </summary>
        /// <param name="name">Name of category</param>
        /// <returns>True if specified and False if not</returns>
        public bool CategoryExists(string name)
        {
            if (createdCategories.Contains(name)) return true;
            return false;
        }

        /// <summary>
        ///     Gets current category
        /// </summary>
        /// <returns>Current category or default category if not created</returns>
        public string GetCurrentCategory()
        {
            string tmp;
            if (CategorySpecified()) tmp = category;
            else tmp = Main;
            return tmp;
        }
    }
}