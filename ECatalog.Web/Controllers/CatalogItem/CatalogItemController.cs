using ECatalog.Application.Common;
using ECatalog.Application.DTO;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace ECatalog.Web.Controllers.CatalogItem
{
    [Route("/CatalogItem")]
    public class CatalogItemController : Controller
    {
        private readonly HttpClient _http;

        public CatalogItemController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7069/");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetCatalogItem")]
        public async Task<IActionResult> GetCatalogItem()
        {
            var response = await _http.GetAsync($"api/CatalogItem");
            return Json(await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<CatalogItemDTO>>>());
        }

        [HttpGet("CreatePartial")]
        public async Task<IActionResult> CreatePartial()
        {
            var dto = new CatalogItemDTO();
            return PartialView("_CatalogItemForm", dto);
        }

        [HttpGet("EditPartial/{id}")]
        public async Task<IActionResult> EditPartial(Guid id)
        {
            var response = await _http.GetAsync($"api/CatalogItem/{id}");
            var dto = await response.Content.ReadFromJsonAsync<ApiResponse<CatalogItemDTO?>>();
            return PartialView("_CatalogItemForm", dto.Data);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CatalogItemDTO dto)
        {
            var response = await _http.PostAsJsonAsync($"api/CatalogItem", dto);
            return Json(await response.Content.ReadFromJsonAsync<ApiResponse<Guid>>());
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CatalogItemDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/CatalogItem/{id}", dto);
            return Json(await response.Content.ReadFromJsonAsync<ApiResponse<bool>>());
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _http.DeleteAsync($"api/CatalogItem/{id}");
            return Json(await response.Content.ReadFromJsonAsync<ApiResponse<bool>>());
        }
    }
}
