using Real.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Real.Model
{
    class EDIDModel
    {
        /////////////menu Model
        public ICommand btnfile { get; set; }

        public string edid { get; set; }

        public string sorts { get; set; }

        public string parser { get; set; }

        public bool plusDot { get; set; }

        public bool plusHex { get; set; }

        public bool reVisibility { get; set; }

        public bool nameVisibility { get; set; }

        public bool saveVisibility { get; set; }
        /////////////
        /////////////read Model
        public List<monitor> M { get; set; }

        public int indexmonitor { get; set; }

        public ICommand selectedid { get; set; }

        public string readedid { get; set; }
        /// /////////
        /// <summary>
        /// //////////change Mdoel
        /// </summary>
        public string hSyns { get; set; }

        public string hFront { get; set; }

        public string hActive { get; set; }

        public string hBack { get; set; }

        public string vSyns { get; set; }

        public string vFront { get; set; }

        public string vActive { get; set; }

        public string vBack { get; set; }

        public string pi { get; set; }

        public int index { get; set; }

        public ICommand changeEdid { get; set; }
        ////////////
        ////////////NameChange Model
        public string aName { get; set; }

        public ICommand changeName { get; set; }
        ////////////
        ////////////write Model
        public ICommand writeParser { get; set; }

        public string parserName { get; set; }

        public bool? txt { get; set; }

        public bool? bin { get; set; }

        public ICommand saveFile { get; set; }
        ////////////
    }
}
