using System;
using System.Data;
using Npgsql;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        var connString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=mydatabase";

        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();
        // read data from the "kunden" table
        await using (var kundenCmd = new NpgsqlCommand("SELECT * FROM kunde", conn))
        await using (var kundenReader = await kundenCmd.ExecuteReaderAsync())
        {
            Console.WriteLine("Kunde:");
            while (await kundenReader.ReadAsync())
            {
                Console.WriteLine($"{kundenReader["kundenid"]}, {kundenReader["kundennr"]}, {kundenReader["name"]}, {kundenReader["strasse"]}, {kundenReader["ort"]}");
            }
        }
        // read data from the "artikel" table
        await using (var artikelCmd = new NpgsqlCommand("SELECT * FROM artikel", conn))
        await using (var artikelReader = await artikelCmd.ExecuteReaderAsync())
        {
            Console.WriteLine("Artikel:");
            while (await artikelReader.ReadAsync())
            {
                Console.WriteLine($"{artikelReader["artikelid"]}, {artikelReader["artikelnr"]}, {artikelReader["name"]}, {artikelReader["preis"]}");
            }
        }
        // read data from the "rechnung" table
        await using (var rechnungCmd = new NpgsqlCommand("SELECT * FROM rechnung", conn))
        await using (var rechnungReader = await rechnungCmd.ExecuteReaderAsync())
        {
            Console.WriteLine("Rechnung:");
            while (await rechnungReader.ReadAsync())
            {
                Console.WriteLine($"{rechnungReader["rechnungid"]}, {rechnungReader["kundenid"]}, {rechnungReader["artikelid"]}, {rechnungReader["anzahl"]}");
            }
        }
        GesamtbetragProKunde(conn);
    }
    public static void GesamtbetragProKunde(NpgsqlConnection conn)
    {
        using (var cmd = new NpgsqlCommand("SELECT k.kundenid, k.name AS kundenname, SUM(r.anzahl * a.preis) AS gesamtbetrag " +
                                          "FROM kunde k " +
                                          "JOIN rechnung r ON k.kundenid = r.kundenid " +
                                          "JOIN artikel a ON r.artikelid = a.artikelid " +
                                          "GROUP BY k.kundenid, k.name", conn))
        using (var reader = cmd.ExecuteReader())
        {
            Console.WriteLine("Gesamtbetrag pro Kunde:");
            while (reader.Read())
            {
                int kundenId = reader.GetInt32(0);
                string kundenname = reader.GetString(1);
                double gesamtbetrag = reader.GetDouble(2);

                Console.WriteLine($"Kunde: {kundenname}, Gesamtbetrag: {gesamtbetrag:C}");
            }
        }
    }
}

