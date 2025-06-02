
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeLavant_CourseWeb.Models;

public class LectureIn {
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? FileType { get; set; }
    public  IFormFile? LectureFile { get; set; }
}