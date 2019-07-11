using Rhetos.Configuration.Autofac;
using Rhetos.Logging;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDump;
using System.Threading;
using Common;

namespace ConsoleApp1
{
    class Program
    {

       public static void generirajSobe(DomRepository repository) {

            var noveSobe = new List<Rezervacije.Soba>();
            for (int i = 1; i < 11; i++)
            {
                var hotel = repository.Rezervacije.Hotel.Load().FirstOrDefault();
                var tipSobe = repository.Rezervacije.TipSobe.Load().FirstOrDefault();

                var novaSoba = new Rezervacije.Soba {
                    Name= String.Format("Generirana soba {0}",i),
                    HotelID = hotel.ID,
                    RoomTypeID =tipSobe.ID
                };
                noveSobe.Add(novaSoba);
            }
            repository.Rezervacije.Soba.Insert(noveSobe);

        }

        static void Main(string[] args)
        {
            ConsoleLogger.MinLevel = EventType.Info; // Use "Trace" for more details log.
            var rhetosServerPath = @"C:\2019\RhetosEdukacija\PrvaRadionica\Hotel\dist\HotelRhetosServer";
            Directory.SetCurrentDirectory(rhetosServerPath);
            using (var container = new RhetosTestContainer(commitChanges: true)) // Use this parameter to COMMIT or ROLLBACK the data changes.
            {
                var context = container.Resolve<Common.ExecutionContext>();
                var repository = context.Repository;

                // Query data from the `Common.Claim` table:
                /*
                var claims = repository.Common.Claim.Query()
                    .Where(c => c.ClaimResource.StartsWith("Common.") && c.ClaimRight == "New")
                    .ToSimple(); // Removes ORM navigation properties from the loaded objects.

                claims.ToString().Dump("Common.Claims SQL query");
                claims.Dump("Common.Claims items");

                // Add and remove a `Common.Principal`:

                var testUser = new Common.Principal { Name = "Test123", ID = Guid.NewGuid() };
                repository.Common.Principal.Insert(new[] { testUser });
                repository.Common.Principal.Delete(new[] { testUser });

                // Print logged events for the `Common.Principal`:

                repository.Common.LogReader.Query()
                    .Where(log => log.TableName == "Common.Principal" && log.ItemId == testUser.ID)
                    .ToList()
                    .Dump("Common.Principal log");

                Console.WriteLine("Done.");
                */
                /*
                var knjige = repository.Knjiznica.Knjiga.Load(x => x.BrojStranica<50).Dump();
                var knjige1 = repository.Knjiznica.Knjiga.Query(x => x.BrojStranica<50).ToList().Dump("1");
                */


                var filter = new Knjiznica.CommonMisspelling();
                var autori = repository.Knjiznica.Knjiga.Query().Select(x => x.Autor.Ime).ToList().Dump();
                var sql = repository.Knjiznica.Knjiga.Query(filter).ToString().Dump();
                repository.Knjiznica.Knjiga.Query(new[] { new Guid() }).Dump();


                var fil = new FilterCriteria("Naslov", "StartsWith", "p");
                repository.Knjiznica.Knjiga.Query(fil).Dump();

                var novaKnjiga = new Knjiznica.Knjiga
                {
                    BrojStranica = 10,
                    Naslov = "Knjiga iz consolne aplikacije"
                };

                repository.Knjiznica.Knjiga.Insert(novaKnjiga);

                /*
                 * lock radi provjeru objekta na bazi ne onoga  što mu šaljemo s forme
                var tipGosta = repository.Rezervacije.TipGosta.Load(new[] { new Guid("E8392299-400F-4F9C-969B-1F9196E77834") }).SingleOrDefault();
                tipGosta.Name = "e" + tipGosta.Name;
                tipGosta.Dump();
                repository.Rezervacije.TipGosta.Update(tipGosta);
               */

               
                var novaSoba = new Rezervacije.Soba { Name = "1", RoomTypeID = new Guid("ddf4aa2d-75fd-43b2-99d4-c9d77f67a483") };

                if (repository.Rezervacije.Soba.Load(new[] { novaSoba.ID}).Count() == 0) { 
                    repository.Rezervacije.Soba.Insert(novaSoba);
                }

                var noviGost = new Rezervacije.Gost {
                    ID=new Guid("B31439D5-EB14-4979-A3DF-E678929602D6"),
                    GuestTypeID = new Guid("568552AB-E1C9-4657-8B87-3D68E7B39223")
                };
                //repository.Rezervacije.Gost.Insert(noviGost);

                
                var rezervacija = new Rezervacije.Rezervacija {
                    CheckIn = DateTime.Now,
                    CheckOut = DateTime.Now.AddDays(1),
                    GuestID= new Guid("B31439D5-EB14-4979-A3DF-E678929602D6"),
                    RoomID = new Guid("37E9B885-C30E-4F8A-B006-E0A975ACCB1B")
               
                };

                

               repository.Rezervacije.Rezervacija.Insert(rezervacija);
                //rezervacija.Dump();
                Thread.Sleep(1000);
                rezervacija.CheckOut = DateTime.Now.AddDays(2);
                repository.Rezervacije.Rezervacija.Update(rezervacija);


                //var aktivneRezervacije = repository.Rezervacije.Rezervacija.Filter(new ActiveItems()); 


                var sobe = repository.Rezervacije.Soba.Load().Select(s => s.BrojSobe).Dump();

                var hotelSoba = repository.Rezervacije.Soba.Query().Select(s => s.BrojSobe + s.Hotel.Name).ToString().Dump();


                //  generirajSobe(repository);

                //var sobe = repository.Rezervacije.Soba.Query(x=> x.)
                // var knjiga = repository.Knjiznica.Knjiga.Query(x=> x.Extension_StranaKnjiga)



                repository.Knjiznica.Knjiga.Query().Where(k => k.Naslov.Contains("post")).ToSimple().ToArray();

                var filterMinBrojStranica = new Knjiznica.Pretrazivanje() { MinBrojStranica = 1 };

                repository.Knjiznica.Knjiga.Query(filterMinBrojStranica).Dump();

            }
            Console.ReadKey();
        }
    }
}
