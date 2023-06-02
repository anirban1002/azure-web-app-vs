using azure_storage_account.Data;
using azure_storage_account.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace azure_storage_account.Controllers
{
    public class AttendeeRegistrationController : Controller
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly IBlobStorageService _blobStorageService;

        public AttendeeRegistrationController(ITableStorageService tableStorageService, IBlobStorageService blobStorageService)
        {
            _tableStorageService = tableStorageService;
            _blobStorageService = blobStorageService;
        }
        // GET: AttendeeRegistrationController
        public async Task<ActionResult> Index()
        {
            var data = await _tableStorageService.GetAttendees();
            foreach (var item in data)
            {
                item.ImageName = await _blobStorageService.GetBlobUrl(item.ImageName);
            }
            return View(data);
        }

        // GET: AttendeeRegistrationController/Details/5
        public async Task<ActionResult> Details(string id, string industry)
        {
            var data = await _tableStorageService.GetAttendee(industry, id);
            data.ImageName = await _blobStorageService.GetBlobUrl(data.ImageName);
            return View(data);
        }

        // GET: AttendeeRegistrationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttendeeRegistrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AttendeeEntity attendeeEntity, IFormFile formFile)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                attendeeEntity.PartitionKey = attendeeEntity.Industry;
                attendeeEntity.RowKey = id;

                if (formFile?.Length > 0)
                {
                    attendeeEntity.ImageName =
                        await _blobStorageService.UploadBlob(formFile, id);
                }
                else
                {
                    attendeeEntity.ImageName = "default.jpg";
                }

                await _tableStorageService.UpsertAttendee(attendeeEntity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendeeRegistrationController/Edit/5
        public async Task<ActionResult> Edit(string id, string industry)
        {
            var data = await _tableStorageService.GetAttendee(industry, id);
            return View(data);
        }

        // POST: AttendeeRegistrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AttendeeEntity attendeeEntity, IFormFile formFile)
        {
            try
            {
                if (formFile?.Length > 0)
                {
                    attendeeEntity.ImageName =
                        await _blobStorageService.UploadBlob(formFile, attendeeEntity.RowKey,attendeeEntity.ImageName);
                }
                attendeeEntity.PartitionKey = attendeeEntity.Industry;

                await _tableStorageService.UpsertAttendee(attendeeEntity);



                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: AttendeeRegistrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, string industry)
        {
            try
            {
                var data = await _tableStorageService.GetAttendee(industry, id);
                await _tableStorageService.DeleteAttendee(industry, id);
                await _blobStorageService.RemoveBlob(data.ImageName);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
