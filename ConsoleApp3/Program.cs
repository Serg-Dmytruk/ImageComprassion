using System.Drawing;
using ConsoleApp3.Handlers;
using ConsoleApp3.Options;

List<Image> images = [];
var imagesPath = Directory.GetFiles(ImageRouts.InRout, "*.jpg");

images.AddRange(imagesPath.Select(path => Image.FromFile(path)));

ImageHandler.SaveImages(images);