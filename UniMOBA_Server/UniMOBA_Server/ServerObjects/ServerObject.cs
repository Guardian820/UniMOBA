using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniMOBA_Server
{
    public class ServerObject
    {
        public ServerObject parent { get; private set; }
        List<ServerObject> childs;

        Dictionary<Type, object> components;

        public ServerObject(ServerObject parent)
        {
            this.parent = parent;
            childs = new List<ServerObject>();
            components = new Dictionary<Type, object>();
        }

        public void AddChild(ServerObject child)
        {
            childs.Add(child);
        }

        public T GetChild<T>(Enum _enum) where T : ServerObject
        {
            return (T)childs[(int)(object)_enum];
        }

        public List<T> GetChilds<T>() where T : ServerObject
        {
            return childs.ConvertAll(x => (T)x);
        }

        public void AddComponent(object component)
        {
            components.Add(component.GetType(), component);
        }

        public T GetComponent<T>()
        {
            return (T)components[typeof(T)];
        }
    }
}
