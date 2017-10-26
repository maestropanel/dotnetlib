namespace MaestroPanel.Api
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("Result")]
    public class ApiResult<T>
    {
        /// <summary>
        /// Return operation status.
        /// 
        /// </summary>
        public int StatusCode { get; set; }


        /// <summary>
        ///İstenilen eylemin başarılı olup olmadığının bilgisini tutar. 0 (sıfır) ise istenilen eylem başarılı sayılır. 
        ///Sıfırdan farklı ise hata ile ilgili "Message" kısmında bilgilendirme yapılır.
        ///
        ///Sık karşılaşılan geri dönüş kodları aşağıda tablo halinde verilmiştir.
        ///
        ///-1
        ///İşlem başarısız. Genel Hata. Bu hata kodunda detay görmek için Details alanına bakılabilir.
        ///Constant: DOMAIN_OPERATION_ONERROR
        ///0
        ///İşlem başarılı.
        ///Constant: DOMAIN_OPERATION_SUCCESS
        ///5
        ///Kimlik doğrulama işlemi geçersiz.
        ///Constant: API_AUTHENTICATION_ERROR
        ///6
        ///Beklenen parametre eksik veya boş.
        ///Constant: API_PARAMETER_ERROR
        ///7
        ///İstenilen eylemin gerçekleşmesi için kullanıcı hakları 
        ///yetersiz.
        ///Constant: API_ACCESS_DENIED
        ///8
        ///İstenilen domain adı yanlış veya domain sistemde yok
        ///Constant: API_DOMAIN_NOT_FOUND
        // </summary>
        public int ErrorCode { get; set; }      
  
        public string Message { get; set; }

        public T Details { get; set; }
    }
}
