Dapper guide:

- It need parameterless ctor or a ctor that accepts all result fields as parameter.
- It supports private ctors
- It supports properties with private setters
- It does not support fields.
- The object to me mapped can contain less properties that the query result (as long there is parameterless ctor).
- The object may contain extra properties. They'll have default values.
- It does not have support for nullable parameters, in context won't modify the query to "is null". We have to construct the query so it has support for null. Example:
```sql
Select * from Customer where (@name is not null AND Name = @name) OR (@name is null AND Name is null)
```
- If the column names in DB tables doesn't match the property names, we can set mappings. We can use either attributes (I won't recommenr) or do it manually. Refer to Sample7. The sample is constructed in such a way that we don't need to map the column name that match, only the different ones.
- 