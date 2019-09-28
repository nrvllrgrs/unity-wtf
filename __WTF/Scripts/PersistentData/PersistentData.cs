using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using Sirenix.Serialization;
using Ludiq;

#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
#endif

namespace UnityEngine.Workshop
{
	[IncludeInSettings(true)]
	public static class PersistentData
	{
		#region Variables

		private static Dictionary<string, Dictionary<string, object>> s_data;
		private static string DEFAULT_FILENAME = "data";
		public static readonly string PASSWORD = "S=N=jgkRXcS7gk_u";

		#endregion

		#region Properties

		private static Dictionary<string, Dictionary<string, object>> data
		{
			get
			{
				if (s_data == null)
				{
					s_data = new Dictionary<string, Dictionary<string, object>>();

					var obj = new GameObject("PersistentDataSaver");
					obj.hideFlags |= HideFlags.HideAndDontSave;
					Object.DontDestroyOnLoad(obj);

					obj.AddComponent<PersistentDataSaver>();
				}
				return s_data;
			}
		}

		#endregion

		#region Getter Methods

		public static bool TryGet<T>(string key, out T value)
		{
			return TryGet(DEFAULT_FILENAME, key, out value);
		}

		public static bool TryGet<T>(string fileName, string key, out T value)
		{
			if (Exists(fileName, key))
			{
				value = (T)data[fileName][key];
				return true;
			}

			value = default;
			return false;
		}

		public static object Get(string key, object fallback)
		{
			return Get(DEFAULT_FILENAME, key, fallback);
		}

		public static object Get(string fileName, string key, object fallback)
		{
#if UNITY_EDITOR

			if (!Application.isPlaying)
			{
				return fallback;
			}

#endif
			if (TryGet(key, out object value))
			{
				return value;
			}

			Set(fileName, key, fallback);
			return fallback;
		}

		public static T Get<T>(string key, T fallback)
		{
			return Get(DEFAULT_FILENAME, key, fallback);
		}

		public static T Get<T>(string fileName, string key, T fallback)
		{
#if UNITY_EDITOR

			if (!Application.isPlaying)
			{
				return fallback;
			}

#endif
			if (TryGet(fileName, key, out T value))
			{
				return value;
			}

			Set(fileName, key, fallback);
			return fallback;
		}

		#endregion

		#region Setter Methods

		public static void Set(string key, object value)
		{
			Set(DEFAULT_FILENAME, key, value);
		}

		public static void Set(string fileName, string key, object value)
		{
			GetEntry(fileName).Set(key, value);
		}

		public static void Set<T>(string key, T value)
		{
			Set(DEFAULT_FILENAME, key, value);
		}

		public static void Set<T>(string fileName, string key, T value)
		{
			GetEntry(fileName).Set(key, value);
		}

		#endregion

		#region Misc Methods

		public static void Reset<T>(string key)
		{
			Reset<T>(DEFAULT_FILENAME, key);
		}

		public static void Reset<T>(string fileName, string key)
		{
			if (Exists(fileName, key))
			{
				Set<T>(fileName, key, default(T));
			}
		}

		public static void Delete()
		{
			Delete(DEFAULT_FILENAME);
		}

		public static void Delete(string fileName)
		{
			if (EntryExists(fileName))
			{
				data[fileName].Clear();

				// Delete local file
				if (FileExists(fileName))
				{
					File.Delete(GetFilePath(fileName));
				}
			}
		}

		#endregion

		#region Helper Methods

		public static bool FileExists(string fileName)
		{
			return File.Exists(GetFilePath(fileName));
		}

		private static Dictionary<string, object> GetEntry(string fileName)
		{
			if (!EntryExists(fileName))
			{
#if UNITY_EDITOR
				Load(fileName);
#else
                Load(fileName, PASSWORD);
#endif
			}

			return data[fileName];
		}

		private static bool EntryExists(string fileName)
		{
			return data.ContainsKey(fileName);
		}

		public static bool Exists(string fileName, string key)
		{
			return GetEntry(fileName).ContainsKey(key);
		}

		public static bool Exists(string key)
		{
			return Exists(DEFAULT_FILENAME, key);
		}

		#endregion

		#region IO Methods

		private static string GetFilePath(string fileName = null)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				fileName = DEFAULT_FILENAME;
			}

#if UNITY_EDITOR
			return string.Format("{0}/Editor/{1}.json", Application.persistentDataPath, fileName);
#else
			return string.Format("{0}/{1}.json", Application.persistentDataPath, fileName);
#endif
		}

		public static void Load(string fileName, string password = null)
		{
			Dictionary<string, object> entry;

			string filePath = GetFilePath(fileName);
			if (File.Exists(filePath))
			{
				List<Object> objectReferences = new List<Object>();
				{
					byte[] bytes = File.ReadAllBytes(filePath);
					if (!string.IsNullOrWhiteSpace(password))
					{
						bytes = Decrypt(bytes, password);
					}

					entry = SerializationUtility.DeserializeValue<Dictionary<string, object>>(
						bytes,
						DataFormat.JSON,
						objectReferences);
				}
			}
			else
			{
				entry = new Dictionary<string, object>();
			}

			data.Set(fileName, entry);
		}

		public static void Load()
		{
			string directoryPath;
#if UNITY_EDITOR
			directoryPath = Path.Combine(Application.persistentDataPath, "Editor");
#else
			directoryPath = Application.persistentDataPath;
#endif

			foreach (var path in Directory.GetFiles(directoryPath, "*.json", SearchOption.TopDirectoryOnly))
			{
				string fileName = Path.GetFileNameWithoutExtension(path);
#if UNITY_EDITOR
				Load(fileName);
#else
				Load(fileName, PASSWORD);
#endif
			}
		}

		public static void Save(string fileName, string password = null)
		{
			if (!EntryExists(fileName))
				return;

			string filePath = GetFilePath(fileName);
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));

			List<Object> objectReferences = new List<Object>();
			{
				byte[] bytes = SerializationUtility.SerializeValue(
					GetEntry(fileName),
					DataFormat.JSON,
					out objectReferences);

				if (!string.IsNullOrWhiteSpace(password))
				{
					bytes = Encrypt(bytes, password);
				}

				File.WriteAllBytes(filePath, bytes);
			}
		}

		public static void Save()
		{
			foreach (string fileName in data.Keys)
			{
#if UNITY_EDITOR
				Save(fileName);
#else
				Save(fileName, PASSWORD);
#endif
			}
		}

		private static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
		{
			MemoryStream memoryStream = new MemoryStream();

			Rijndael alg = Rijndael.Create();
			alg.Key = key;
			alg.IV = iv;

			CryptoStream cryptoStream = new CryptoStream(memoryStream, alg.CreateEncryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(data, 0, data.Length);
			cryptoStream.Close();

			return memoryStream.ToArray();
		}

		private static byte[] Encrypt(byte[] data, string password)
		{
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(
				password,
				new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

			return Encrypt(data, pdb.GetBytes(32), pdb.GetBytes(16));
		}

		private static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
		{
			MemoryStream memoryStream = new MemoryStream();

			Rijndael alg = Rijndael.Create();
			alg.Key = key;
			alg.IV = iv;

			CryptoStream cryptoStream = new CryptoStream(memoryStream, alg.CreateDecryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(data, 0, data.Length);
			cryptoStream.Close();

			return memoryStream.ToArray();
		}

		private static byte[] Decrypt(byte[] data, string password)
		{
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(
				password,
				new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

			return Decrypt(data, pdb.GetBytes(32), pdb.GetBytes(16));
		}

		#endregion

		#region Editor Methods
#if UNITY_EDITOR

		[MenuItem("Tools/Odin Serializer/Open Containing Folder")]
		private static void OpenContainingFolder()
		{
			Process.Start(Application.persistentDataPath);
		}

#endif
		#endregion
	}

	public class PersistentDataSaver : MonoBehaviour
	{
		#region Methods

		private void OnApplicationQuit()
		{
			PersistentData.Save();
		}

		#endregion
	}
}
