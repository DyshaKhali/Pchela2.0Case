using Microsoft.AspNetCore.Mvc;
using Npgsql;
using pchela2._0_case.Models;
using System.Configuration;
using System.Diagnostics;

namespace pchela2._0_case.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = GetClientData();

            return View(model);
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private List<Client> GetClientData()
        {
            var clientData = new List<Client>();

            using (var connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=root;"))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM pchela2.pchela_people", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var clientModel = new Client
                            {
                                ClientID = reader.GetInt32(0),
                                NameClient = reader.GetString(1),
                                RegistrationDate = reader.GetDateTime(2),
                                Country = reader.GetString(3),
                                City = reader.GetString(4),
                                Email = reader.GetString(5),
                                PhoneNumber = reader.GetString(6),
                                TotalPurchaseAmount = reader.GetDecimal(7),
                                CustomerCategory = reader.GetString(8),
                                ActiveStatus = reader.GetString(9),
                                LastVisitDate = reader.GetDateTime(10),
                                CommunicationMethods = (string[])reader["CommunicationMethods"],
                                AdditionalContacts = (string[])reader["AdditionalContacts"],
                                CustomerPreferences = reader.GetString(13),
                                LanguageSpeak = reader.GetString(14),
                                MaritalStatus = reader.GetString(15),
                                Notes = reader.GetString(16),
                                SatisfactionLevel = reader.GetDecimal(17),
                                LastUpdateDate = reader.GetDateTime(18),
                                CustomFields = reader.GetString(19)
                            };


                            clientData.Add(clientModel);
                        }
                    }
                }
            }

            return clientData;
        }

        
        public IActionResult FilterClients(string id, string fullName, DateTime? regDate, string country, string city, string email, string phone, decimal? purAmount, string cusCategory, string actStatus, DateTime? lastVisit, string comMethod, string additContact, string cusPrefer, string lang, string marStatus, string note, decimal? satisLvl, DateTime? lastUpdate, string cusField)
        {

            var model = GetFilteredClients(id, fullName, regDate, country, city, email, phone, purAmount, cusCategory, actStatus, lastVisit, comMethod, additContact, cusPrefer, lang, marStatus, note, satisLvl, lastUpdate, cusField);

            return PartialView("_ClientTable", model);
        }


        protected List<Client> GetFilteredClients(string id, string fullName, DateTime? regDate, string country, string city, string email, string phone, decimal? purAmount, string cusCategory, string actStatus, DateTime? lastVisit, string comMethod, string additContact, string cusPrefer, string lang, string marStatus, string note, decimal? satisLvl, DateTime? lastUpdate, string cusField)
        {
            List<Client> clients = new List<Client>();

            
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=pchela2;Username=pchela2;Password=pchela;"))
                {
                    connection.Open();

                    string query = "SELECT * FROM public.pchela_people_dayte_deneg WHERE 1=1";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        // условия фильтрации
                        if (!string.IsNullOrEmpty(id))
                        {
                            query += " AND ClientID = @Id";
                            command.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                        }
                        if (!string.IsNullOrEmpty(fullName))
                        {
                            query += " AND NameClient ILIKE @FullName";
                            command.Parameters.AddWithValue("@FullName", "%" + fullName + "%");
                        }
                        if (regDate.HasValue)
                        {
                            query += " AND RegistrationDate::date = @RegDate";
                            command.Parameters.AddWithValue("@RegDate", regDate.Value.Date);
                        }
                        if (!string.IsNullOrEmpty(country))
                        {
                            query += " AND Country ILIKE @Country";
                            command.Parameters.AddWithValue("@Country", "%" + country + "%");
                        }
                        if (!string.IsNullOrEmpty(city))
                        {
                            query += " AND City ILIKE @City";
                            command.Parameters.AddWithValue("@City", "%" + city + "%");
                        }
                        if (!string.IsNullOrEmpty(email))
                        {
                            query += " AND Email ILIKE @Email";
                            command.Parameters.AddWithValue("@Email", "%" + email + "%");
                        }
                        if (!string.IsNullOrEmpty(phone))
                        {
                            query += " AND PhoneNumber ILIKE @Phone";
                            command.Parameters.AddWithValue("@Phone", "%" + phone + "%");
                        }
                        if (purAmount.HasValue)
                        {
                            query += " AND TotalPurchaseAmount ILIKE @PurAmount";
                            command.Parameters.AddWithValue("@PurAmount", "%" + purAmount.Value + "%");
                        }
                        if (!string.IsNullOrEmpty(cusCategory))
                        {
                            query += " AND CustomerCategory ILIKE @CusCategory";
                            command.Parameters.AddWithValue("@CusCategory", "%" + cusCategory + "%");
                        }
                        if (!string.IsNullOrEmpty(actStatus))
                        {
                            query += " AND ActiveStatus ILIKE @ActStatus";
                            command.Parameters.AddWithValue("@ActStatus", "%" + actStatus + "%");
                        }
                        if (lastVisit.HasValue)
                        {
                            query += " AND LastVisitDate::date = @LastVisit";
                            command.Parameters.AddWithValue("@LastVisit", lastVisit.Value.Date);
                        }
                        if (!string.IsNullOrEmpty(comMethod))
                        {
                            query += " AND @ComMethod = ANY(CommunicationMethods)";
                            command.Parameters.AddWithValue("@ComMethod", comMethod);
                        }
                        if (!string.IsNullOrEmpty(additContact))
                        {
                            query += " AND @AdditContact = ANY(AdditionalContacts)";
                            command.Parameters.AddWithValue("@AdditContact", additContact);
                        }
                        if (!string.IsNullOrEmpty(cusPrefer))
                        {
                            query += " AND CustomerPreferences ILIKE @CusPrefer";
                            command.Parameters.AddWithValue("@CusPrefer", "%" + cusPrefer + "%");
                        }
                        if (!string.IsNullOrEmpty(lang))
                        {
                            query += " AND LanguageSpeak ILIKE @Lang";
                            command.Parameters.AddWithValue("@Lang", "%" + lang + "%");
                        }
                        if (!string.IsNullOrEmpty(marStatus))
                        {
                            query += " AND MaritalStatus ILIKE @MarStatus";
                            command.Parameters.AddWithValue("@MarStatus", "%" + marStatus + "%");
                        }
                        if (!string.IsNullOrEmpty(note))
                        {
                            query += " AND Notes ILIKE @Note";
                            command.Parameters.AddWithValue("@Note", "%" + note + "%");
                        }
                        if (satisLvl.HasValue)
                        {
                            query += " AND SatisfactionLevel = @SatisLvl";
                            command.Parameters.AddWithValue("@SatisLvl", satisLvl.Value);
                        }
                        if (lastUpdate.HasValue)
                        {
                            query += " AND LastUpdateDate::date = @LastUpdate";
                            command.Parameters.AddWithValue("@LastUpdate", lastUpdate.Value.Date);
                        }
                        if (!string.IsNullOrEmpty(cusField))
                        {
                            query += " AND CustomFields ILIKE @CusField";
                            command.Parameters.AddWithValue("@CusField", "%" + cusField + "%");
                        }

                        command.CommandText = query;

                        NpgsqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Client client = new Client
                            {
                                ClientID = reader.GetInt32(0),
                                NameClient = reader.GetString(1),
                                RegistrationDate = reader.GetDateTime(2),
                                Country = reader.GetString(3),
                                City = reader.GetString(4),
                                Email = reader.GetString(5),
                                PhoneNumber = reader.GetString(6),
                                TotalPurchaseAmount = reader.GetDecimal(7),
                                CustomerCategory = reader.GetString(8),
                                ActiveStatus = reader.GetString(9),
                                LastVisitDate = reader.GetDateTime(10),
                                CommunicationMethods = (string[])reader["CommunicationMethods"],
                                AdditionalContacts = (string[])reader["AdditionalContacts"],
                                CustomerPreferences = reader.GetString(13),
                                LanguageSpeak = reader.GetString(14),
                                MaritalStatus = reader.GetString(15),
                                Notes = reader.GetString(16),
                                SatisfactionLevel = reader.GetDecimal(17),
                                LastUpdateDate = reader.GetDateTime(18),
                                CustomFields = reader.GetString(19)
                            };

                            clients.Add(client);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, логирование и т. д.
                Console.WriteLine("Произошла ошибка при получении фильтрованных клиентов: " + ex.Message);
            }

            return clients;
        }


    }
}