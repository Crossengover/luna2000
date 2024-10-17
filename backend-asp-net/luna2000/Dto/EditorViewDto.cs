namespace luna2000.Dto;

public class EditorViewDto
{
    public string?[] FileNames { get; set; }

    public (Guid driverId, string name)[] Drivers { get; set; }

    public (Guid carId, string name)[] Cars { get; set; }
}
