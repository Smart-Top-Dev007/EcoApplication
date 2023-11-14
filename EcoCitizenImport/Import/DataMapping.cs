using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoCitizenImport.Import
{
	internal class PropertyMapping
	{
		internal string ColumnName { get; }

		internal int Column { get; set; }

		internal PropertyMapping(string columnName)
		{
			ColumnName = columnName;
		}
	}

	internal class PropertyValue
	{
		internal string ColumnName { get; }

		internal object Value { get; set; }

		internal PropertyValue(string columnName, object value)
		{
			ColumnName = columnName;
			Value = value;
		}
	}

	internal class WorksheetDataRow : Dictionary<string, PropertyValue>
	{
		public override string ToString()
		{
			return string.Join("|", Values.Select(v => $"{v.ColumnName}:{v.Value}"));
		}
	}

	internal class DataMapping : IEnumerable, IEnumerable<PropertyMapping>
	{
		private readonly Dictionary<string, PropertyMapping> _properties = new Dictionary<string, PropertyMapping>();

		internal PropertyMapping this[string columnName] =>
			_properties.ContainsKey(columnName) ? _properties[columnName] : null;

		internal void SetColumn(string columnName, int column)
		{
			if (_properties.ContainsKey(columnName))
			{
				_properties[columnName].Column = column;
			}
			else
			{
				_properties.Add(columnName, new PropertyMapping(columnName) { Column = column });
			}
		}

		internal int Count => _properties.Count;

		internal PropertyMapping[] Properties => _properties.Values.ToArray();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _properties.GetEnumerator();
		}

		public IEnumerator<PropertyMapping> GetEnumerator()
		{
			foreach (var pm in _properties.Values)
			{
				yield return pm;
			}
		}

		public override string ToString()
		{
			return string.Join("|", _properties.Select(p => $"{p.Value.ColumnName}:{p.Value.Column}"));
		}
	}
}
