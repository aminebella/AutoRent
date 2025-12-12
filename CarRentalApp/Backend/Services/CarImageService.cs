using backendclienttesting.Backend.Models;
using CarRentalApp.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Backend.Services
{
    public class CarImageService
    {
        private readonly CarImageDao imgDao;
        private readonly string imageFolder = "Images";

        public CarImageService()
        {
            imgDao = new CarImageDao();

            // Find directory For images
            string baseDir = Directory.GetCurrentDirectory();
            string solutionDir = Directory.GetParent(baseDir).Parent.Parent.FullName;
            // "../Images"
            imageFolder = Path.Combine(solutionDir, "Images");

            // Ensure Images folder exists
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);
        }

        // Show: Get all images for a car
        public List<CarImage> GetImagesByCarId(int carId) =>
            imgDao.GetImagesByCarId(carId);

        // Add: Add image
        public bool AddImage(int carId, string path)
        {
            if (!File.Exists(path))
                return false; // if the image path given doesn't exist

            string fileName = Guid.NewGuid() + Path.GetExtension(path); // generates a unique random image name to prevent names conflict
            string destPath = Path.Combine(imageFolder, fileName); 

            File.Copy(path, destPath, overwrite: true);

            var img = new CarImage
            {
                CarId = carId,
                ImagePath = destPath,
            };
            return imgDao.AddImage(img);
        }

        // Delete Partial: Delete one image
        public bool DeleteOneImageById(int imageId)
        {
            // 1. Get image info from DB
            var img = imgDao.GetImageById(imageId);
            if (img == null)
                return false;

            // 2. Delete file from disk
            if (File.Exists(img.ImagePath))
                File.Delete(img.ImagePath);

            // 3. Delete DB record
            return imgDao.DeleteOneImageById(imageId);
        }

        // Delete Total: Delete all images of a car
        public bool DeleteAllImagesOfCar(int carId)
        {
            // 1. Get all images for the car
            var images = imgDao.GetImagesByCarId(carId);

            // 2. Delete every file
            foreach (var img in images)
            {
                if (File.Exists(img.ImagePath))
                    File.Delete(img.ImagePath);
            }

            // 3. Delete all DB records
            return imgDao.DeleteAllImagesByCarId(carId);
        }
    }
}
