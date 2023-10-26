using System;
//Global Co-ordinates system
public class GCS
{
    private double longitude;
    private double latitude;
    public double Longitude
    {
        get { return longitude; }
        set { longitude = value; }
    }
    public double Latitude
    {
        get { return latitude; }
        set { latitude = value; }
    }
    public GCS() {
        this.latitude = 0;
        this.longitude = 0;
    }
    public GCS(double lon, double lat) {
        this.latitude = lat;
        this.longitude = lon;
    }
    public string toString() {
        string longLet = "S, ";
        string latLet = "W";
        if (longitude >= 0) longLet = "N, ";
        if (latitude >= 0) latLet = "E";
        return splitDegree(Math.Abs(longitude)) + longLet 
            + splitDegree(Math.Abs(latitude)) + latLet;
    }

    private string splitDegree(double coord) {
        double degree = Math.Floor(coord);
        double minutes = (coord - degree) * 60;
        double seconds = (minutes - Math.Floor(minutes)) * 60;
        string coordstr = String.Format("{0}°{1}'{2}\"", 
            (int) degree, (int) minutes, (int) seconds);
        return coordstr;
    }
}
