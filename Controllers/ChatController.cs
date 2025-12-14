using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebPetShop.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        // Danh sách câu hỏi – câu trả lời (kịch bản)
        private static readonly List<(string Key, string Value)> Scripts = new()
        {
            ("bé ăn gì chưa", "Dạ bé ăn rồi nha chị ❤️ Hôm nay bé ăn rất ngoan ạ."),
            ("bé ăn chưa", "Dạ bé ăn rồi nha chị ❤️ Bé khỏe lắm ạ."),
            ("bé ngủ chưa", "Bé vừa chợp mắt xíu rồi ạ 😴❤️"),
            ("bé sao rồi", "Bé khỏe mạnh và rất ngoan nha chị ❤️"),
            ("tình trạng bé sao rồi", "Hiện tại bé hoàn toàn bình thường và vui vẻ ạ 🐶❤️"),
            ("hôm nay bé có ngoan không", "Dạ bé ngoan lắm luôn chị ơi 🐾❤️"),
            ("bé có tắm chưa", "Dạ chiều nay tụi em tắm cho bé rồi nha, thơm thiệt thơm luôn ạ 🛁✨"),
            ("bé có uống thuốc chưa", "Dạ bé đã uống thuốc đúng giờ luôn ạ 💊❤️"),
            ("bé hôm nay thế nào", "Bé hoạt động tốt, ăn ngủ điều độ và rất dễ thương ạ 😍"),
            ("hello", "Dạ em chào anh/chị nha ❤️ Em có thể giúp gì ạ?"),
            ("hi", "Dạ em chào anh/chị nha ❤️ Em có thể hỗ trợ gì cho mình ạ?"),
            ("bé ăn gì chưa", "Dạ bé ăn rồi nha chị ❤️ Bé ăn rất ngon và sạch sẽ luôn ạ."),
            ("bé ăn chưa", "Dạ bé vừa ăn xong ạ, ăn rất ngoan luôn chị ❤️"),
            ("bé ăn nhiều không", "Hôm nay bé ăn tốt hơn hôm qua luôn ạ 🐶❤️"),
            ("bé có kén ăn không", "Bé không kén ăn đâu chị, bé ăn rất hợp tác luôn đó ❤️"),
            ("bé có uống nước không", "Dạ bé uống đầy đủ nước ạ, tụi em luôn để nước sạch sẵn cho bé ❤️"),
            ("bé ăn món gì", "Hôm nay bé ăn pate + hạt mềm nha chị, cực kỳ thích luôn ạ 😍"),
            ("bé có bỏ ăn không", "Không đâu ạ ❤️ Bé ăn bình thường và rất ngoan."),
            ("bé ăn được bao nhiêu", "Bé ăn gần hết khẩu phần tụi em chuẩn bị luôn ạ ❤️"),
            ("bé có ăn hết không", "Dạ hôm nay bé ăn sạch bách luôn chị 😆❤️"),
            ("bé có dị ứng thức ăn không", "Tụi em theo dõi rồi, chưa thấy dấu hiệu dị ứng nào nha chị ❤️"),
            ("bé ngủ chưa", "Bé vừa chợp mắt xíu rồi ạ 😴❤️"),
            ("bé ngủ có ngon không", "Bé ngủ rất sâu và thoải mái luôn chị ❤️"),
            ("bé có quậy không", "Không đâu ạ, bé rất ngoan và thân thiện luôn 🐾❤️"),
            ("bé hôm nay có ngoan không", "Bé ngoan cực kỳ luôn chị, ai cũng thương hết ạ 😍"),
            ("bé có stress không", "Hiện tại bé rất ổn định, không có dấu hiệu căng thẳng nha chị ❤️"),
            ("bé có mệt không", "Dạ bé không mệt ạ, hoạt động bình thường và rất vui vẻ ❤️"),
            ("bé vui không", "Bé hôm nay rất vui, chạy chơi nhiều lắm chị ơi 🐶💖"),
            ("bé có sợ không", "Ban đầu hơi rụt rè xíu nhưng giờ quen môi trường rồi ạ ❤️"),
            ("bé có buồn không", "Tụi em luôn chơi với bé nên bé không buồn đâu chị ❤️"),
            ("bé có khóc không", "Không đâu ạ, bé rất bình tĩnh và dễ thương luôn 😘"),
            ("bé có tắm chưa", "Chiều nay tụi em tắm cho bé rồi luôn, thơm thiệt thơm ạ 🛁✨"),
            ("bé có được tắm không", "Có ạ, tùy gói ký gửi nhưng tụi em tắm bé đúng lịch nha chị ❤️"),
            ("bé có ị không", "Dạ bé đi vệ sinh bình thường ạ, phân đẹp không có vấn đề gì ❤️"),
            ("bé có vệ sinh sạch không", "Tụi em dọn khu vực liên tục nên lúc nào cũng sạch sẽ ạ ✨"),
            ("bé có bị tiêu chảy không", "Hiện tại bé đi vệ sinh hoàn toàn bình thường, không tiêu chảy ạ ❤️"),
            ("bé có bị hôi không", "Tụi em vệ sinh chuồng thường xuyên nên bé và khu vực đều sạch sẽ nha chị ❤️"),
            ("chuồng có sạch không", "Dạ chuồng bé được khử trùng mỗi ngày luôn ạ ✨"),
            ("bé có bị ve rận không", "Tụi em kiểm tra rồi, bé không có ve rận nha chị ❤️"),
            ("bé có được chải lông không", "Tụi em chải lông nhẹ cho bé mỗi ngày để bé luôn thoải mái nha 💕"),
            ("bé có dùng cát sạch không", "Có ạ, tụi em dùng cát vệ sinh loại tốt, thay thường xuyên cho mèo nha 😺❤️"),
            ("bé có uống thuốc chưa", "Dạ bé uống thuốc đúng giờ rồi ạ 💊❤️"),
            ("bé có bệnh không", "Tụi em theo dõi rồi, hiện không có dấu hiệu bệnh nào nha chị ❤️"),
            ("bé có sốt không", "Không đâu ạ, thân nhiệt bé hoàn toàn bình thường ❤️"),
            ("bé có ho không", "Không ạ, bé thở đều và khỏe mạnh ❤️"),
            ("bé có nôn không", "Tụi em quan sát suốt và không thấy bé nôn đâu chị ❤️"),
            ("bé có bị thương không", "Không có vết thương nào nha chị, bé an toàn tuyệt đối ❤️"),
            ("bé có được theo dõi không", "Tụi em theo sát bé liên tục mỗi ngày luôn ạ ❤️"),
            ("bé tiêm chưa", "Nếu chị chọn gói có tiêm phòng, tụi em sẽ tiêm đúng lịch và báo lại ngay ạ ❤️"),
            ("tình trạng sức khỏe bé sao rồi", "Bé hoàn toàn bình thường, khỏe mạnh và hoạt bát nha chị ❤️"),
            ("bé có uống đủ nước không", "Dạ có ạ, tụi em luôn đảm bảo bé không bị thiếu nước ❤️"),
            ("bé có chơi không", "Có ạ, hôm nay bé chơi nhiều lắm, còn chạy vòng vòng nữa 😆❤️"),
            ("bé có chạy không", "Dạ có ạ, bé rất năng động và dễ thương ❤️"),
            ("bé có cắn phá không", "Không đâu ạ, bé rất ngoan và nghe lời ❤️"),
            ("bé có sợ người lạ không", "Ban đầu hơi rụt rè nhưng giờ bé quen hết nhân viên rồi ạ 🥰"),
            ("bé có thân thiện không", "Bé cực kỳ thân thiện luôn, ai gặp cũng thương ạ ❤️"),
            ("bé có được chơi với bạn khác không", "Nếu chị cho phép, tụi em sẽ cho bé giao lưu an toàn nha 🐶🐶❤️"),
            ("bé có nhớ chủ không", "Em nghĩ là có đó ạ 🥺 Nhưng tụi em thương bé nhiều nên bé ổn lắm chị ❤️"),
            ("bé có được dắt đi dạo không", "Có ạ, dắt đi dạo theo gói ký gửi chị chọn nha ❤️"),
            ("bé có cào không", "Không đâu ạ, bé rất nhẹ nhàng luôn 😺❤️"),
            ("bé có hung dữ không", "Không ạ, bé hiền khô à chị ơi 🥰"),
            ("bé nôn", "Dạ tụi em quan sát rồi ạ. Bé có nôn nhẹ nhưng hiện đã ổn lại rồi chị ơi ❤️"),
            ("bé ói", "Bé có ói nhẹ ạ, thường do bé ăn nhanh hoặc đổi môi trường. Hiện bé đã bình thường và tụi em đang chăm kỹ lắm chị ❤️"),
            ("bé tiêu chảy", "Cũng may là hiện bé chỉ đi phân hơi mềm thôi ạ, chưa phải tiêu chảy. Tụi em đang theo dõi chế độ ăn để bé ổn định lại ❤️"),
            ("bé bỏ ăn", "Hôm nay bé hơi biếng ăn xíu ạ, nhưng bé vẫn uống nước và hoạt động bình thường ❤️"),
            ("bé bị đầy bụng", "Tụi em mát-xa nhẹ cho bé rồi ạ ❤️ Bé hiện đã thoải mái hơn."),
            ("bé ho", "Dạ bé có ho nhẹ ạ, em đang theo dõi xem có phải do thời tiết thay đổi không."),
            ("bé khò khè", "Bé có tiếng khò nhẹ ạ, thường là do bé nằm ngủ sai tư thế 😊"),
            ("bé sổ mũi", "Bé hơi sổ mũi chút xíu thôi chị, tụi em đang giữ ấm và vệ sinh mũi cho bé ❤️"),
            ("bé thở mạnh", "Dạ bé thở hơi nhanh là do vừa vận động xong ạ 🐶"),
            ("bé ngạt mũi", "Tụi em có nhỏ nước muối sinh lý cho bé rồi ạ 😊"),
            ("bé ngứa", "Dạ bé có gãi nhẹ ạ, tụi em đã kiểm tra không có ve rận ❤️"),
            ("bé bị dị ứng", "Bé hơi đỏ da nhẹ ạ, có thể là kích ứng. Tụi em đã xử lý và theo dõi ❤️"),
            ("bé rụng lông", "Rụng lông nhẹ là bình thường khi thay lông chị nha 🐾"),
            ("bé có ve", "Tụi em kiểm tra kỹ rồi ạ, hiện không thấy dấu hiệu ve rận ❤️"),
            ("bé bị mẩn đỏ", "Bé có vài nốt đỏ nhỏ ạ, đang vệ sinh và theo dõi thêm ❤️"),
            ("bé mệt", "Dạ bé hơi mệt nhẹ do thay đổi môi trường ạ."),
            ("bé lừ đừ", "Tụi em thấy bé ít chạy hơn hôm qua nhưng ăn uống tốt ❤️"),
            ("bé đau", "Chị yên tâm ạ, tụi em kiểm tra rồi bé không đau ❤️"),
            ("bé đi loạng choạng", "Có thể bé vừa ngủ dậy nên hơi loạng choạng xíu 😅"),
            ("bé run", "Bé run nhẹ do hơi sợ khi mới gặp người lạ ❤️ Tụi em đã trấn an bé rồi ❤️")
        };

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] ChatRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Message))
                return Ok(new { reply = "Dạ em chưa nhận được tin nhắn ạ ❤️" });

            string userMsg = req.Message.ToLower().Trim();

            // Tìm câu trả lời phù hợp
            var match = Scripts.FirstOrDefault(x => userMsg.Contains(x.Key));

            if (!string.IsNullOrEmpty(match.Key))
                return Ok(new { reply = match.Value });

            // Nếu không khớp → trả lời mặc định
            return Ok(new
            {
                reply = "Dạ em nhận được rồi ạ ❤️ Anh/chị mô tả rõ hơn giúp em để em hỗ trợ tốt nhất nha 🐶"
            });
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}
