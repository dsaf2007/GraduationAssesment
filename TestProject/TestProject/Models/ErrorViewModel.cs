namespace TestProject.Models
{
    /// <summary>
    /// 에러 뷰 모델
    /// </summary>
    public class ErrorViewModel
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 요청 ID - RequestID

        /// <summary>
        /// 요청 ID
        /// </summary>
        public string RequestID { get; set; }

        #endregion
        #region 요청 ID 표시 여부 - ShowRequestID

        /// <summary>
        /// 요청 ID 표시 여부
        /// </summary>
        public bool ShowRequestID => !string.IsNullOrEmpty(RequestID);

        #endregion
    }
}