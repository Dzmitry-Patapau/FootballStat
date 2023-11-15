namespace FootballStats.Services
{
    public class ImageService: IImageService
    {
        private IWebHostEnvironment _webHostEnvironment { get; }

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile _imageFile, string _pathImage)
        {
            if (_imageFile == null || _pathImage == null)
            {
                return null;
            }
            var fileExtension = Path.GetExtension(_imageFile.FileName);
            var upload = Path.Combine(_webHostEnvironment.WebRootPath, _pathImage);
            var uniqName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(upload, uniqName);

            using (var stream = new FileStream(filePath,FileMode.Create))
            {
                await _imageFile.CopyToAsync(stream);
            }

            return $"/{_pathImage}/" + uniqName;
        }
    }
}
