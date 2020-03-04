using System;
using System.Linq;
using Dapper;

namespace App.DL.Model.Image {
    public class Image : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public string email;

        public string url;

        public DateTime created_at;

        public static Image Find(int id)
            => Connection().Query<Image>(
                "SELECT * FROM images WHERE id = @id LIMIT 1", new {id}
            ).FirstOrDefault();
        
        public static Image FindBy(string col, string val)
            => Connection().Query<Image>(
                $"SELECT * FROM images WHERE {col} = @val LIMIT 1", new {val}
            ).FirstOrDefault();

        public static int Create(string url) {
            return ExecuteScalarInt(
                @"INSERT INTO public.images(guid, url) VALUES (@guid, @url); SELECT currval('images_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), url}
            );
        }

        public void Delete() => ExecuteScalarInt("DELETE FROM images WHERE id = @id", new {id});
    }
}