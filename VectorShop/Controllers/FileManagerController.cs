using System.IO;
using System.Linq;
using System.Web.Mvc;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class FileManagerController : Controller
    {
        [Route("FileManager/{subFolder?}")]
        public virtual ActionResult Files(string subFolder)
        {        // FileViewModel contains the root MyFolder and the selected subfolder if any
            FileViewModel model = new FileViewModel() { Folder = "MyFolder", SubFolder = subFolder };

            return View(model);
        }
    }
}
