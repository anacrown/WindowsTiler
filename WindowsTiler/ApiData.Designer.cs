// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 5.2.97.0. www.xsd2code.com
//    {"TargetFramework":"Net20","NameSpace":"WindowsTiler","Properties":{},"ClassParams":{},"Miscellaneous":{}}
//  </auto-generated>
// ------------------------------------------------------------------------------
#pragma warning disable
namespace WindowsTiler
{
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Runtime.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.Xml;
    using System.Collections.Generic;


    [DebuggerStepThrough]
    public partial class window
    {
        #region Private fields
        private List<windowPosition> _position;
        private List<windowSize> _size;
        private List<windowCondition> _condition;
        private string _process;
        private windowMode _mode;
        #endregion

        public window()
        {
            _condition = new List<windowCondition>();
            _size = new List<windowSize>();
            _position = new List<windowPosition>();
        }

        public List<windowPosition> position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public List<windowSize> size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public List<windowCondition> condition
        {
            get
            {
                return _condition;
            }
            set
            {
                _condition = value;
            }
        }

        public string process
        {
            get
            {
                return _process;
            }
            set
            {
                _process = value;
            }
        }

        public windowMode mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }
    }

    [DebuggerStepThrough]
    public partial class windowPosition
    {
        #region Private fields
        private ushort _x;
        private byte _y;
        #endregion

        public ushort X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public byte Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
    }

    [DebuggerStepThrough]
    public partial class windowSize
    {
        #region Private fields
        private ushort _width;
        private ushort _height;
        #endregion

        public ushort width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public ushort height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
    }

    [DebuggerStepThrough]
    public partial class windowCondition
    {
        #region Private fields
        private List<windowConditionTitle> _title;
        private windowConditionWidth _width;
        private windowConditionHeight _height;
        #endregion

        public windowCondition()
        {
            _height = new windowConditionHeight();
            _width = new windowConditionWidth();
            _title = new List<windowConditionTitle>();
        }

        public List<windowConditionTitle> title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public windowConditionWidth width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public windowConditionHeight height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }
    }

    [DebuggerStepThrough]
    public partial class windowConditionTitle
    {
        #region Private fields
        private string _value;
        private bool _isempty;
        private windowConditionTitleMode _mode;
        #endregion

        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public bool isempty
        {
            get
            {
                return _isempty;
            }
            set
            {
                _isempty = value;
            }
        }

        public windowConditionTitleMode mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }
    }

    public enum windowConditionTitleMode
    {
        equals,
        notequals,
    }

    [DebuggerStepThrough]
    public partial class windowConditionWidth
    {
        #region Private fields
        private uint _value;
        private uint _accuracy;
        #endregion

        public uint value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public uint accuracy
        {
            get
            {
                return _accuracy;
            }
            set
            {
                _accuracy = value;
            }
        }
    }

    [DebuggerStepThrough]
    public partial class windowConditionHeight
    {
        #region Private fields
        private uint _value;
        private uint _accuracy;
        #endregion

        public uint value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public uint accuracy
        {
            get
            {
                return _accuracy;
            }
            set
            {
                _accuracy = value;
            }
        }
    }

    public enum windowMode
    {
        hold,
        close,
        remember,
        topmost,
        notopmost,
    }
}
#pragma warning restore
