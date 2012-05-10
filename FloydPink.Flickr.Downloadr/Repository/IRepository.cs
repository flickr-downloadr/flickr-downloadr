using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FloydPink.Flickr.Downloadr.Model;

namespace FloydPink.Flickr.Downloadr.Repository
{
    public interface IRepository<T> 
    {
        T Get();
        void Save(T token);
        void Delete();
    }
}
