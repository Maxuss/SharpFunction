using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFunction.Exceptions
{
    /// <summary>
    /// Thrown when could not find .sfmeta file in project
    /// </summary>
    public class MetaFileNotFound : SFException
    {
        /// <inheritdoc cref="SFException"/>
        public MetaFileNotFound() : base("Could not locate .sfmeta project file!") { }
    }
}
