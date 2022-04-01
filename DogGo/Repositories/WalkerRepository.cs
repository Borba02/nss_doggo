using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId, n.Name as NeighborhoodName
                        FROM Walker w
                        LEFT JOIN Neighborhood n ON w.NeighborhoodId = n.Id
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood
                                {
                                    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName")),
                                }
                            };

                            walkers.Add(walker);
                        }

                        return walkers;
                    }
                }
            }
        }

        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId, Neighborhood.Name as NeighborhoodName
                        FROM Walker w
                        LEFT JOIN Neighborhood ON w.NeighborhoodId = Neighborhood.Id
                        WHERE w.Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood
                                {
                                    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName")),
                                }
                            };

                            return walker;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        public void AddWalker(Walker walker)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            INSERT INTO Walker
                                ([Name], ImageUrl, NeighborhoodId)
                                OUTPUT INSERTED.ID
                                VALUES (@name, @imageUrl, @neighborhoodId);";
                    cmd.Parameters.AddWithValue("@name", walker.Name);
                    cmd.Parameters.AddWithValue("@imageUrl", walker.ImageUrl == null ? "https://simg.nicepng.com/png/small/274-2744819_people-silhouette-avatar-business-man-icon.png" : walker.ImageUrl);
                    cmd.Parameters.AddWithValue("@neighborhoodId", walker.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();

                    walker.Id = id;
                }
            }
        }
        public void UpdateWalker(Walker walker)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Walker
                                   SET [Name]= @name,
                                       NeighborhoodId = @neighborhoodId,
                                       ImageUrl = @imageUrl
                                   WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", walker.Name);
                    cmd.Parameters.AddWithValue("@neighborhoodId", walker.NeighborhoodId);
                    cmd.Parameters.AddWithValue("@imageUrl", walker.ImageUrl == null ? "https://simg.nicepng.com/png/small/274-2744819_people-silhouette-avatar-business-man-icon.png" : walker.ImageUrl);
                    cmd.Parameters.AddWithValue("@id", walker.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteWalker(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Walker
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", walkerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, [Name], ImageUrl, NeighborhoodId
                FROM Walker
                WHERE NeighborhoodId = @neighborhoodId
            ";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            };

                            walkers.Add(walker);
                        }

                        return walkers;
                    }
                }
            }
        }
        
    }
}