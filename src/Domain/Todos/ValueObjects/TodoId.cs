using System.ComponentModel;
using System.Text.Json.Serialization;
using StrongTypedId;
using StrongTypedId.Converters;

namespace Domain.Todos.ValueObjects;

[TypeConverter(typeof(StrongTypedIdTypeConverter<TodoId, Guid>))]
[JsonConverter(typeof(SystemTextJsonConverter<TodoId, Guid>))]
public class TodoId : StrongTypedId<TodoId, Guid>
{
	public TodoId(Guid primitiveId) : base(primitiveId)
	{
	}
}