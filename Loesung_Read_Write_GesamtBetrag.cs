using System;
using Npgsql;

public class ArtikelModel
{
    public int ArtikelId { get; set; }
    public int ArtikelNr { get; set; }
    public string Name { get; set; }
    public double Preis { get; set; }
}
public class RechnungModel
{
    public int RechnungId { get; set; }
    public int KundenId { get; set; }
    public int ArtikelId { get; set; }
    public int Anzahl { get; set; }
}
public class KundeModel
{
    public int KundenId { get; set; }
    public int KundenNr { get; set; }
    public string Name { get; set; }
    public string Strasse { get; set; }
    public string Ort { get; set; }
}
class Program
{
    static void Main()
    {
        string connString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=mydatabase";

        RechnungModel rechnung = new RechnungModel
        {
            KundenId = 1,
            ArtikelId = 1,
            Anzahl = 3,
        };

        ArtikelModel artikel = new ArtikelModel
        {
            ArtikelNr = 1,
            Name = "AEG",
            Preis = 3000,
        };

        KundeModel kunde = new KundeModel
        {
            KundenNr = 1,
            Name = "Denis",
            Strasse = "Denisstr",
            Ort = "DD1"
        };
        // DATEN lesen
        dataRechnung(connString, rechnung);
        dataArtikel(connString, artikel);
        dataKunde(connString, kunde);
        // DATEN hinzufuegen
        insertKunde(connString, kunde);
        insertArtikel(connString, artikel);
        insertRechnung (connString, rechnung);
        // Gesamtbetrag rechnen
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();
            GesamtbetragProKunde(conn);
            conn.Close();
        }
    }

    static void dataRechnung(string connString, RechnungModel rechnung)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("UPDATE rechnung SET kundenid = @kundenid, artikelid = @artikelid, anzahl = @anzahl WHERE rechnungid = @rechnungid", conn))
            {
                cmd.Parameters.AddWithValue("kundenid", rechnung.KundenId);
                cmd.Parameters.AddWithValue("artikelid", rechnung.ArtikelId);
                cmd.Parameters.AddWithValue("anzahl", rechnung.Anzahl);
                cmd.Parameters.AddWithValue("rechnungid", rechnung.RechnungId);

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }
    }

    static void dataArtikel(string connString, ArtikelModel artikel)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("UPDATE artikel SET artikelnr = @artikelnr, name = @name, preis = @preis WHERE artikelid = @artikelid", conn))
            {
                cmd.Parameters.AddWithValue("artikelnr", artikel.ArtikelNr);
                cmd.Parameters.AddWithValue("name", artikel.Name);
                cmd.Parameters.AddWithValue("preis", artikel.Preis);
                cmd.Parameters.AddWithValue("artikelid", artikel.ArtikelId);

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }
    }
    
    static void dataKunde(string connString, KundeModel kunde)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("UPDATE kunde SET name = @name, strasse = @strasse, ort = @ort WHERE kundenNr = @kundenNr RETURNING kundenId", conn))
            {
                cmd.Parameters.AddWithValue("kundenNr", kunde.KundenNr);
                cmd.Parameters.AddWithValue("name", kunde.Name);
                cmd.Parameters.AddWithValue("strasse", kunde.Strasse);
                cmd.Parameters.AddWithValue("ort", kunde.Ort);

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }
    }
    static void insertRechnung(string connString, RechnungModel rechnung)
    {   
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("INSERT INTO rechnung (kundenid, artikelid, anzahl) VALUES (@kundenid, @artikelid, @anzahl) RETURNING rechnungid", conn))
            {
                cmd.Parameters.AddWithValue("kundenid", rechnung.KundenId);
                cmd.Parameters.AddWithValue("artikelid", rechnung.ArtikelId);
                cmd.Parameters.AddWithValue("anzahl", rechnung.Anzahl);

                rechnung.RechnungId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            conn.Close();
        }
    }

    static void insertArtikel(string connString, ArtikelModel artikel)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("INSERT INTO artikel (artikelnr, name, preis) VALUES (@artikelnr, @name, @preis) RETURNING artikelid", conn))
            {
                cmd.Parameters.AddWithValue("artikelnr", artikel.ArtikelNr);
                cmd.Parameters.AddWithValue("name", artikel.Name);
                cmd.Parameters.AddWithValue("preis", artikel.Preis);

                artikel.ArtikelId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            conn.Close();
        }
    }

    static void insertKunde(string connString, KundeModel kunde)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        {
            conn.Open();

            using (var cmd = new NpgsqlCommand("INSERT INTO kunde (kundenNr, name, strasse, ort) VALUES (@kundenNr, @name, @strasse, @ort) RETURNING kundenId", conn))
            {
                cmd.Parameters.AddWithValue("kundenNr", kunde.KundenNr);
                cmd.Parameters.AddWithValue("name", kunde.Name);
                cmd.Parameters.AddWithValue("strasse", kunde.Strasse);
                cmd.Parameters.AddWithValue("ort", kunde.Ort);

                kunde.KundenId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            conn.Close();
        }
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
            Console.WriteLine("---------------Gesamtbetrag--------------");
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
