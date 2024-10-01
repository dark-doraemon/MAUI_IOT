using SQLite;
using MAUI_IOT.Models;
using MAUI_IOT.Models.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
#if ANDROID
using Android.OS;
using Android.Content;
#endif


namespace MAUI_IOT.Services.Implements.DataManagement
{
    public class DatabaseHelper
    {
        public static string Database_path { get; private set; } = string.Empty;
        private static SQLiteAsyncConnection _connection = null;
        public static void InitConnection(string fileName)
        {
            if (_connection != null) { return; }
            Task.Run(async () => await CheckAndRequestStoragePermission());
            string dbPath = string.Empty;
#if ANDROID
            dbPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, fileName);
#elif WINDOWS
            dbPath = Path.Combine(FileSystem.AppDataDirectory, fileName);
#else
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
#endif

            Database_path = dbPath;
            _connection = new SQLiteAsyncConnection(dbPath);
            try
            {
                _connection.CreateTableAsync<ExperimentManager>().Wait();
                _connection.CreateTableAsync<Data>().Wait();
                _connection.CreateTableAsync<Experiment>().Wait();
                _connection.CreateTableAsync<ExperimentConfig>().Wait();
                _connection.CreateTableAsync<DataSummarize>().Wait();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo bảng: {ex.Message}");
            }
        }

        public static void AddAsync(ExperimentManager data)
        {
            try
            {
                if (_connection == null)
                {
                    System.Diagnostics.Debug.WriteLine("Database is not connected");
                }
                else
                {
                    _connection.InsertAsync(data).Wait();
                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            }
        }

        public static async Task AddAsync(Data data)
        {
            if(_connection == null)
            {
                System.Diagnostics.Debug.WriteLine("Database is not connected");
            }
            else
            {
                await _connection.InsertAsync(data);
            }
        }

        public static async Task AddAsync(Experiment data)
        {
            if (_connection == null)
            {
                System.Diagnostics.Debug.WriteLine("Database is not connected");
            }
            else
            {
                await _connection.InsertAsync(data);
            }
        }

        public static async Task AddAsync(ExperimentConfig data)
        {
            if (_connection == null)
            {
                System.Diagnostics.Debug.WriteLine("Database is not connected");
            }
            else
            {
                await _connection.InsertAsync(data);
            }
        }

        public static async Task AddAsync(DataSummarize data)
        {
            if (_connection == null)
            {
                System.Diagnostics.Debug.WriteLine("Database is not connected");
            }
            else
            {
                await _connection.InsertAsync(data);
            }
        }

        public static async Task<ObservableCollection<ExperimentManager>> GetExperimentManagersAsync()
        {
            if (_connection != null)
            {
                var dataList = await _connection.Table<ExperimentManager>().ToListAsync();
                return new ObservableCollection<ExperimentManager>(dataList);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Database is not connect");
                return new ObservableCollection<ExperimentManager>();
            }
        }

        public static async Task<ObservableCollection<Experiment>> GetExperimentsByExperimentManagerId(string id)
        {
            var dataList = await _connection.Table<Experiment>().Where(d => d.ExperimentManagerId == id).ToListAsync();
            return new ObservableCollection<Experiment>(dataList);
        }

        public static async Task<ExperimentConfig> GetExperimentConfigByExperimentId(string id)
        {
            ExperimentConfig dataList = await _connection.Table<ExperimentConfig>()
                                       .Where(data => data.ExperimentId == id)
                                       .FirstOrDefaultAsync();
            return dataList;
        }

        public static async Task<DataSummarize> GetDataSummarizeByExperimentId(string id)
        {
            DataSummarize dataList = await _connection.Table<DataSummarize>()
                                      .Where(data => data.ExperimentId == id)
                                      .FirstOrDefaultAsync();
            return dataList;
        }

        public static async Task<ObservableCollection<Data>> GetDataByExperimentId(string id)
        {
            var dataList = await _connection.Table<Data>()
                                      .Where(data => data.ExperimentId == id)
                                      .ToListAsync();
            return new ObservableCollection<Data>(dataList);
        }

        public static async Task DeleteExperimentManagerById(string id)
        {
            if (_connection == null)
            {
                System.Diagnostics.Debug.WriteLine("Database is not connected");
            }
            else
            {
                var item = await _connection.Table<ExperimentManager>().FirstOrDefaultAsync(x => x.ExperimentManagerId == id);
                if (item != null)
                {
                    await _connection.DeleteAsync(item);
                }
            }
        }

        //Delete Data DataSummarize ExperimentConfig
        public static async Task DeleteByExperimentId(string id)
        {
            if (_connection == null)
            {
                System.Diagnostics.Debug.WriteLine("Database is not connected");
            }
            else
            {
                string sql_1 = "Delete From ExperimentConfig Where ExperimentId = @id";
                await _connection.ExecuteAsync(sql_1, new { ExperimentId = id });
                string sql_2 = "Delete From Data Where ExperimentId = @id";
                await _connection.ExecuteAsync(sql_2, new { ExperimentId = id });
                string sql_3 = "Delete From DataSummarize Where ExperimentId = @id";
                await _connection.ExecuteAsync(sql_3, new { ExperimentId = id });
                System.Diagnostics.Debug.WriteLine("Delete all file has experiment id: " + id);
            }
        }

        private static async Task CheckAndRequestStoragePermission()
        {
            // Trực tiếp yêu cầu quyền

#if ANDROID
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                if (!Android.OS.Environment.IsExternalStorageManager)
                {
                    Intent intent = new Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission);
                    var activity = Platform.CurrentActivity ?? throw new InvalidOperationException("Activity chưa tồn tại");
                    activity.StartActivity(intent);
                    return;
                }
            }
            else
            {
                var status = await Permissions.RequestAsync<Permissions.StorageWrite>();

                if (status != PermissionStatus.Granted)
                {
                    throw new UnauthorizedAccessException("Quyền truy cập lưu trữ chưa được cấp.");
                }
            }
#endif

        }

    }


}
