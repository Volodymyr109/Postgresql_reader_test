using System;
using System.Data;
using Npgsql;
public class KundeModel
{
    public int KundenId { get; set; }
    public int KundenNr { get; set; }
    public string Name { get; set; }
    public string Strasse { get; set; }
    public string Ort { get; set; }

}
public class Kunde : KundeModel
{

    public static void Insert(Kunde kunde, NpgsqlConnection conn)
    {
        using (var cmd = new NpgsqlCommand("INSERT INTO kunde (KundenNr, Name, Strasse, Ort) VALUES (@KundenNr, @Name, @Strasse, @Ort) RETURNING KundenId", conn))
        {
            cmd.Parameters.AddWithValue("KundenNr", kunde.KundenNr);
            cmd.Parameters.AddWithValue("Name", kunde.Name);
            cmd.Parameters.AddWithValue("Strasse", kunde.Strasse);
            cmd.Parameters.AddWithValue("Ort", kunde.Ort);

            kunde.KundenId = Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
    // public static void WriteToCSV(List<Kunde> kundeList, string filePath)
    // {
    //     try
    //     {
    //         using (StreamWriter sw = new StreamWriter(filePath, false))
    //         {
    //             // Schreibe die Header-Zeile in die CSV-Datei
    //             sw.WriteLine("KundenId,KundenNr,Name,Strasse,Ort");

    //             foreach (var kunde in kundeList)
    //             {
    //                 // Schreibe die Daten jeder Zeile in die CSV-Datei
    //                 sw.WriteLine($"{kunde.KundenId},{kunde.KundenNr},{kunde.Name},{kunde.Strasse},{kunde.Ort}");
    //             }

    //             Console.WriteLine("Daten in die Datei geschrieben: " + filePath);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine("Fehler beim Schreiben der Datei: " + ex.Message);
    //     }
    // }
}