using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using F_ckCreditSlavery.Entities.Models.Links;

namespace F_ckCreditSlavery.Entities.DataTransferObjects
{
    public class Entity : DynamicObject, IDictionary<string, object>, IXmlSerializable
    {
        private const string Root = "EntityWithLinks";
        private readonly IDictionary<string, object> _expando;

        public Entity()
        {
            _expando = new ExpandoObject();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_expando.TryGetValue(binder.Name, out object value))
            {
                result = value;
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _expando[binder.Name] = value;

            return true;
        }

        public object this[string key]
        {
            get => _expando[key];
            set => _expando[key] = value;
        }

        public ICollection<string> Keys => _expando.Keys;

        public ICollection<object> Values => _expando.Values;

        public int Count => _expando.Count;

        public bool IsReadOnly => _expando.IsReadOnly;

        public void Add(string key, object value)
        {
            _expando.Add(key, value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _expando.Add(item);
        }

        public void Clear()
        {
            _expando.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _expando.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _expando.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _expando.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _expando.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _expando.Remove(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _expando.Remove(item);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
        {
            return _expando.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(Root);

            while (!reader.Name.Equals(Root))
            {
                var name = reader.Name;

                reader.MoveToAttribute("type");
                var typeContent = reader.ReadContentAsString();
                var underlyingType = Type.GetType(typeContent);
                reader.MoveToContent();
                _expando[name] = reader.ReadElementContentAs(underlyingType!, null);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in _expando.Keys)
            {
                var value = _expando[key];
                WriteXmlElement(key, value, writer);
            }
        }

        private static void WriteXmlElement(string key, object value, XmlWriter writer)
        {
            writer.WriteStartElement(key);

            if (value.GetType() == typeof(List<Link>))
            {
                foreach (var val in (List<Link>) value)
                {
                    writer.WriteStartElement(nameof(Link));
                    WriteXmlElement(nameof(val.Href), val.Href, writer);
                    WriteXmlElement(nameof(val.Method), val.Method, writer);
                    WriteXmlElement(nameof(val.Rel), val.Rel, writer);
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteString(value.ToString());
            }

            writer.WriteEndElement();
        }
    }
}