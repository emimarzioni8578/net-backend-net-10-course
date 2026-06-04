using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var values = new List<ValueDto>
{
    new()
    {
        Id = 1,
        Name = "Primer valor"
    },
    new()
    {
        Id = 2,
        Name = "Segundo valor"
    }
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "MiPrimerApi v1");
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/api/values", () =>
{
    return Results.Ok(values);
})
.WithName("GetAllValues")
.Produces<IEnumerable<ValueDto>>(StatusCodes.Status200OK)
.Produces<ErrorResponse>(StatusCodes.Status404NotFound);

app.MapGet("/api/values/{id:int}", (int id) =>
{
    var value = values.FirstOrDefault(x => x.Id == id);

    if (value is null)
    {
        return Results.NotFound(new ErrorResponse
        {
            Code = "0001",
            Message = "No se encontró el valor esperado"
        });
    }

    return Results.Ok(value);
})
.WithName("GetValueById")
.Produces<ValueDto>(StatusCodes.Status200OK)
.Produces<ErrorResponse>(StatusCodes.Status404NotFound);

app.MapPost("/api/values", (PostValueDto dto) =>
{
    if (string.IsNullOrEmpty(dto.Name))
    {
        return Results.BadRequest(new ErrorResponse
        {
            Code = "0002",
            Message = "El nombre no puede estar vacío"
        });
    }

    var newId = values.Max(v => v.Id) + 1;
    var newValue = new ValueDto
    {
        Id = newId,
        Name = dto.Name
    };

    values.Add(newValue);
    return Results.CreatedAtRoute("GetValueById", new { id = newValue.Id }, newValue);
})
.WithName("CreateValue")
.Produces<ValueDto>(StatusCodes.Status201Created)
.Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

app.MapPut("/api/values/{id:int}", (int id, ValueDto dto) =>
{
    var value = values.FirstOrDefault(x => x.Id == id);

    if (value is null)
    {
        return Results.NotFound(new ErrorResponse
        {
            Code = "0001",
            Message = "No se encontró el valor esperado"
        });
    }

    value.Name = dto.Name;
    return Results.NoContent();
})
.WithName("UpdateValue")
.Produces(StatusCodes.Status204NoContent)
.Produces<ErrorResponse>(StatusCodes.Status404NotFound);

app.MapDelete("/api/values/{id:int}", (int id) =>
{
    var value = values.FirstOrDefault(x => x.Id == id);

    if (value is null)
    {
        return Results.NotFound(new ErrorResponse
        {
            Code = "0001",
            Message = "No se encontró el valor esperado"
        });
    }

    values.Remove(value);
    return Results.NoContent();
})
.WithName("DeleteValue")
.Produces(StatusCodes.Status204NoContent)
.Produces<ErrorResponse>(StatusCodes.Status404NotFound);

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal class ValueDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

internal class PostValueDto
{
    public string? Name { get; set; }
}

internal class ErrorResponse
{
    public ErrorResponse() { }

    public ErrorResponse(string message, string code = "")
    {
        Message = message;
        Code = code;
    }

    public string Message { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
