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
        using (var cmd = new NpgsqlCommand("INSERT INTO kunde (kundennr, name, strasse, ort) VALUES (@kundennr, @name, @strasse, @ort) RETURNING kundenid", conn))
        {
            cmd.Parameters.AddWithValue("kundennr", kunde.KundenNr);
            cmd.Parameters.AddWithValue("name", kunde.Name);
            cmd.Parameters.AddWithValue("strasse", kunde.Strasse);
            cmd.Parameters.AddWithValue("ort", kunde.Ort);

            kunde.KundenId = Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

}