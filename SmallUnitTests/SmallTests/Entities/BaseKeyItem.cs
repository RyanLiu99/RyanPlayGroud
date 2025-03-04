using Ringba.Models.DataModelAttributes;
using System;
using System.Collections.Generic;

namespace Ringba.Models
{
    [LogDedupe]
    public interface IKeyItem
    {
        int Id { get; set; }
    }

    public interface IAccountItem : IKeyItem, IUpdateable
    {
        string AccountId { get; set; }
    }

    public interface IUpdateable
    {
        /// <summary>
        /// reset to true if the model was updated
        /// </summary>
        bool IsUpdated();

        /// <summary>
        /// called on the model after grabing it from Repo, if true, 
        /// update model is called
        /// </summary>
        /// <returns></returns>
        bool ShouldUpdate();

        /// <summary>
        /// updates the model
        /// </summary>
        void UpdateModel();
    }


    [Serializable]
    [ModelVersion(1)]
    public abstract class BaseKeyItem: IAccountItem
    {
        public const int DEFAULT_VERSION = 1;

        private int _version = DEFAULT_VERSION;
        private bool _versionSetExplicitly;
        private bool _updated = false;

        public virtual int Id { get; set; }

        /// <summary>
        /// generation of the model, provied by backend store like Aerospike. 
        /// It should only be set when fetch data from Aerospike.     
        /// </summary>
        public int? generation { get; set; }  //Not at interface level since it does not make sense for other implementations

        public virtual string Name { get; set; }
        public virtual string AccountId { get; set; }
        public virtual bool Enabled { get; set; } = true;
        public override string ToString()
        {
            return string.Format("{0}:{1}", GetType().Name, Id);
        }

        public int Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                _versionSetExplicitly = true;
            }
        }

        public bool IsVersionSetExplicitly()
        {
            return _versionSetExplicitly;
        }

        /// <summary>
        /// only models that overide should update
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldUpdate()
        {
            return false;
        }

        public virtual void UpdateModel()
        {
            
        }

        public virtual bool IsUpdated()
        {
            return _updated;
        }

        public virtual void SetUpdate(bool val)
        {
            _updated = val;
        }


       
    }

    



}
