using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class dangky : MonoBehaviour
{
    public TMP_InputField user;
    public TMP_InputField passwd;
    public TextMeshProUGUI thongbao;
    public void DangKyButton()
    {
        StartCoroutine(DangKy());
    }
    //Đây là phương thức dùng để đăng ký
    private IEnumerator DangKy()
    {
        WWWForm dataForm = new WWWForm(); //khởi tạo 1 form
        dataForm.AddField("user", user.text); //trường User của form
        dataForm.AddField("passwd", passwd.text); //trường password của form
                                                  //Khởi tạo 1 kết nối tới đường dẫn chứa file php
        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangky.php", dataForm);
        yield return www.SendWebRequest(); //Tạm dừng Coroutine và chờ tới khi hoành thành việc kết nối mới tiếp tục chạy đoạn mã dưới 
        if (!www.isDone)
        {
            print("Kết nối không thành công");
        }
        else if (www.isDone)
        {
            string get = www.downloadHandler.text; //Nếu kết nối thành công thì PHP sẽ in ra dòng thông báo và mình lấy về để sử lý 
            
switch (get)
            {
                case "exist": thongbao.text = "Tài khoản đã tồn tại"; break;
                case "OK": thongbao.text = "Đăng ký thành công vui lòng đăng nhập để vào game"; break;
                case "ERROR": thongbao.text = "Đăng ký không thành công"; break;
                default: thongbao.text = "Không kết nối được tới server"; break;
            }
        }
    }

}
