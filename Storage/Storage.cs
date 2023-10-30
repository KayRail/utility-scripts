using Godot;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class Storage {
	
	public static void Delete (string path) {
		if (File.Exists (path)) {
			File.Delete (path);
		}
	}
	
	public static T Load<T> (string path) {
		T result;
		if(string.IsNullOrEmpty(path)) {
			GD.PrintErr("Tried loading without a path!");
			result = default(T);
		} else {
			using (FileStream fStream = File.OpenRead(path)) {
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				result = (T)serializer.Deserialize(fStream);
			}
		}
		return result;
	}

	public static bool Save<T> (T data, string path) {
		bool result;
		try {
			if(string.IsNullOrEmpty(path)) {
                GD.PrintErr("Tried saving without a path!");
				result = false;
			} else if (data == null) {
                GD.PrintErr("Tried saving without data!");
				result = false;
			} else {
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				ns.Add("","");
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				if(serializer == null) {
                    GD.PrintErr("Failed to create a serializer for " + typeof(T).ToString());
					result = false;
				} else {
					Directory.CreateDirectory(Directory.GetParent(path).FullName);
					XmlWriterSettings writerSettings = new XmlWriterSettings{
						OmitXmlDeclaration = true,
						Indent = true
					};
					using (FileStream fStream = new FileStream(path, FileMode.Create)) {
						if(fStream == null) {
                            GD.PrintErr("Failed to create a FileStream for path " + path);
							result = false;
						} else {
							using (XmlWriter writer = XmlWriter.Create(fStream, writerSettings)) {
								if(fStream == null) {
                                    GD.PrintErr("Failed to create a XmlWriter! " + fStream.ToString() + " - " + path);
									result = false;
								} else {
									serializer.Serialize(writer, data, ns);
									result = true;
								}
							}
						}
					}
				}
			}
		} catch (Exception e) {
            GD.PrintErr(e.Message + " while saving!\n" + e.StackTrace);
			result = false;
		}
		
		return result;
	}
}
