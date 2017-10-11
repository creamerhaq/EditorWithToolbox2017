using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoSerializableClassGenerator.ToolboxItems
{
    /// <summary>
    /// This class implements data to be stored in ToolboxItem.
    /// This class needs to be serializable in order to be passed to the toolbox
    /// and back.
    /// 
    /// Moreover, this assembly path is required to be on VS probing paths to make
    /// deserialization successful. See ToolboxData.pkgdef.
    /// </summary>
    [Serializable()]
    public class ToolboxData : ISerializable
    {
        #region Fields
        private string content;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="sentence">Sentence value.</param>
        public ToolboxData(string sentence)
        {
            content = sentence;
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets the ToolboxItemData Content.
        /// </summary>
        public string Content
        {
            get { return content; }
        }
        #endregion Properties

        internal ToolboxData(SerializationInfo info, StreamingContext context)
        {
            this.content = info.GetValue("Content", typeof(string)) as string;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                info.AddValue("Content", Content);
            }
        }
    }
}
