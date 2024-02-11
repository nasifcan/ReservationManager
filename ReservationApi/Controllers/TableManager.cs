using Microsoft.AspNetCore.Mvc;
using ReservationApi.Model;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/[controller]")]
public class TableManager : ControllerBase
{
    private readonly TableService _tableService;

    public TableManager(TableService tableService)
    {
        _tableService = tableService;
    }

    [HttpGet("GetAllTables")]
    [SwaggerOperation(Summary = "GetAllTables")]
    public IActionResult GetAllTables()
    {
        try
        {
            List<Tables> tables = _tableService.GetAllTables();
            if (tables.Count > 0)
            {
                return Ok(tables);
            }
            return StatusCode(500, "Kayıtlı masa bulunmadı.");
        }
        catch (Exception)
        {
            return StatusCode(404, "Masa listesini getirme sırasında hata oluştu.");
        }

    }

    [HttpGet("AddTable")]
    [SwaggerOperation(Summary = "AddTable")]
    public IActionResult AddTable(int number, int capacity)
    {
        try
        {
            if (number <= 0 || capacity <= 0)
            {
                return StatusCode(500, "Masa numarası ve kapasite 0 dan farklı bir değer almalıdır.");
            }
            var table = new Tables
            {
                Number = number,
                Capacity = capacity
            };
            string insertTableResult = _tableService.AddTable(table);
            if (insertTableResult.Contains("HATA"))
            {
                return StatusCode(500, "Masa kaydetme işlemi sırasında bir hata oluştu.");
            }
            return Ok(table);
        }
        catch (Exception)
        {
            return StatusCode(404, "Yeni masa ekleme işlemi sırasında hata oluştu.");
        }
    }

    [HttpPut("UpdateTable")]
    [SwaggerOperation(Summary = "UpdateTable")]
    public IActionResult Update(int id, int number, int capacity)
    {
        try
        {
            if (number <= 0 || capacity <= 0)
            {
                return StatusCode(500, "Masa numarası ve kapasite 0 dan farklı bir değer almalıdır.");
            }
            var table = new Tables
            {
                Id = id,
                Number = number,
                Capacity = capacity
            };
            string insertTableResult = _tableService.UpdateTable(table);
            if (insertTableResult.Contains("HATA"))
            {
                return StatusCode(500, "Masa kaydetme işlemi sırasında bir hata oluştu.");
            }
            return Ok("Masa bilgisini güncelleme işlemi başarılı.");
        }
        catch (Exception)
        {
            return StatusCode(404, "Masa bilgisini güncelleme işlemi sırasında hata oluştu.");
        }
    }

    [HttpDelete("DeleteTable")]
    [SwaggerOperation(Summary = "DeleteTable")]
    public IActionResult Delete(int id)
    {
        try
        {
            List<Tables> list = _tableService.GetAllTables();
            if (list.Count(x => x.Id == id) == 0)
            {
                return StatusCode(500, "Silmek istediğiniz masa sistemde bulunmamaktadır.");
            }
            Tables tables = list.Where(x => x.Id == id).FirstOrDefault();
            string deleteResult = _tableService.DeleteTable(tables);
            if (deleteResult.Contains("HATA"))
            {
                return StatusCode(500, "Masa silme işlemi sırasında bir hata oluştu.");
            }
            return Ok("Silme işlemi başarılı.");
        }
        catch (Exception)
        {
            return StatusCode(404, "Masayı silme işlemi sırasında hata oluştu.");
        }

    }
}