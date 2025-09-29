using System;

namespace TWS.Core.DTOs.Response
{
    /// <summary>
    /// Generic API response wrapper for consistent response structure across all API endpoints
    /// </summary>
    /// <typeparam name="T">Type of data being returned</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the API call was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message describing the result of the API call
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The actual data payload returned by the API
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// HTTP status code for the response
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiResponse()
        {
        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        public ApiResponse(bool success, string message, T data, int statusCode)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        /// <summary>
        /// Creates a successful API response with data
        /// </summary>
        /// <param name="data">The data to return</param>
        /// <param name="message">Optional success message</param>
        /// <param name="statusCode">HTTP status code (default: 200 OK)</param>
        /// <returns>ApiResponse with success status</returns>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Creates an error API response
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="statusCode">HTTP status code (default: 400 Bad Request)</param>
        /// <returns>ApiResponse with error status</returns>
        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default(T),
                StatusCode = statusCode
            };
        }
    }
}