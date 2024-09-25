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
        private SQLiteAsyncConnection _conn;
        public static string Database_path { get; set; }
        public DatabaseHelper(string fileName)
        {

            Task.Run(async () => await CheckAndRequestStoragePermission());

            string dbPath;

#if ANDROID
            dbPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, fileName);
#elif WINDOWS
            dbPath = Path.Combine(FileSystem.AppDataDirectory, fileName);
#else
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
#endif
            _conn = new SQLiteAsyncConnection(dbPath);

            Database_path = dbPath;

            CreateTablesAsync();
        }


        private void CreateTablesAsync()
        {
            try
            {
                _conn.CreateTableAsync<Data>().Wait();
                _conn.CreateTableAsync<Experiment>().Wait();
                _conn.CreateTableAsync<ExperimentInfo>().Wait();
                _conn.CreateTableAsync<DataSummarize>().Wait();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo bảng: {ex.Message}");
            }
        }
        public async Task AddAsync(Data data)
        {
            try
            {
                await _conn.InsertAsync(data);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            }
        }

        public async Task AddAsync(Experiment data)
        {
            await _conn.InsertAsync(data);
        }

        public async Task AddAsync(ExperimentInfo data)
        {
            await _conn.InsertAsync(data);
        }

        public async Task AddAsync(DataSummarize data)
        {
            await _conn.InsertAsync(data);
        }

        public async Task<ObservableCollection<Data>> GetDataByExperimentId(string id)
        {
            var dataList = await _conn.Table<Data>()
                                      .Where(data => data.ExperimentInfoId == id)
                                      .ToListAsync();

            return new ObservableCollection<Data>(dataList);
        }

        public async Task<ObservableCollection<Experiment>> GetExperiments()
        {
            var dataList = await _conn.Table<Experiment>().ToListAsync();
            return new ObservableCollection<Experiment>(dataList);
        }

        public async Task<ObservableCollection<ExperimentInfo>> GetExperimentInfoById(string id)
        {
            var dataList =  await _conn.Table<ExperimentInfo>()
                                       .Where(data => data.ExperimentInfoId == id)
                                       .ToListAsync();
            return new ObservableCollection<ExperimentInfo>(dataList);
        }

        public async Task<ObservableCollection<DataSummarize>> GetDataSummarizeById(string id)
        {
            var dataList = await _conn.Table<DataSummarize>()
                                      .Where(data => data.ExperimentInfoId == id)
                                      .ToListAsync();
            return new ObservableCollection<DataSummarize>(dataList);
        }

        private async Task CheckAndRequestStoragePermission()
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
