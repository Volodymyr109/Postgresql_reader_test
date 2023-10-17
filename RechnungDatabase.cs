using System;
using System.Data;
using Npgsql;
public class RechnungModel
{
    public int RechnungId { get; set; }
    public int KundenId { get; set; }
    public int ArtikelId { get; set; }
    public int Anzahl { get; set; }
}
public class Rechnung : RechnungModel
{
    public static void Insert(Rechnung rechnung, NpgsqlConnection conn)
    {
        using (var cmd = new NpgsqlCommand("INSERT INTO rechnung (kundenid, artikelid, anzahl) VALUES (@kundenid, @artikelid, @anzahl) RETURNING rechnungid", conn))
        {
            cmd.Parameters.AddWithValue("kundenid", rechnung.KundenId);
            cmd.Parameters.AddWithValue("artikelid", rechnung.ArtikelId);
            cmd.Parameters.AddWithValue("anzahl", rechnung.Anzahl);

            rechnung.RechnungId = Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
    public static void WriteToTable(Rechnung rechnung, NpgsqlConnection conn)
    {
        Insert(rechnung, conn);
    }    
}
