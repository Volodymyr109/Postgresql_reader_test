using System;
using System.Data;
using Npgsql;
public class ArtikelModel
{
    public int ArtikelId { get; set; }
    public int ArtikelNr { get; set; }
    public string Name { get; set; }
    public double Preis { get; set; }
}
public class Artikel : ArtikelModel
{
    public static void Insert(Artikel artikel, NpgsqlConnection conn)
    {
        using (var cmd = new NpgsqlCommand("INSERT INTO artikel (artikelnr, name, preis) VALUES (@artikelnr, @name, @preis) RETURNING artikelid", conn))
        {
            cmd.Parameters.AddWithValue("artikelnr", artikel.ArtikelNr);
            cmd.Parameters.AddWithValue("name", artikel.Name);
            cmd.Parameters.AddWithValue("preis", artikel.Preis);

            artikel.ArtikelId = Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}