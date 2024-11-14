<script>
    // Đảm bảo rằng vocabList đã được chuyển đúng sang JavaScript
    const vocabList = @Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Model));
    let currentIndex = 0;

    // Hàm để cập nhật flashcard với dữ liệu từ vocabList
    function updateFlashcard() {
        const vocabulary = vocabList[currentIndex];
    console.log(vocabulary);  // Kiểm tra xem vocab có giá trị đúng không
    if (!vocabulary) {
        console.error("Dữ liệu vocab không hợp lệ.");
    return;
        }

    document.getElementById("word").textContent = vocabulary.Word;
    const partOfSpeech = vocabulary.PartOfSpeech ? vocabulary.PartOfSpeech.toString() : "Unknown";
    document.getElementById("partOfSpeech").textContent = partOfSpeech.charAt(0).toUpperCase() + partOfSpeech.slice(1);
    document.getElementById("meaning").textContent = vocabulary.Meaning;
    document.getElementById("exampleSentence").textContent = vocabulary.ExampleSentence;

    const audioElement = document.getElementById("audio");
    const audioSource = document.getElementById("audioSource");
    if (vocabulary.AudioFile) {
        audioSource.src = `/audio/${vocabulary.AudioFile}`;
    audioElement.style.display = "block";
        } else {
        audioSource.src = "";
    audioElement.style.display = "none";
        }
    audioElement.load();
    document.getElementById("flashcard").classList.remove("flipped");
    updateButtons();
    }

    // Hàm cập nhật nút điều hướng
    function updateButtons() {
        document.getElementById("prevBtn").disabled = currentIndex === 0;
    document.getElementById("nextBtn").disabled = currentIndex === vocabList.length - 1;
    }

    // Hàm chuyển tới flashcard tiếp theo
    function showNext() {
        if (currentIndex < vocabList.length - 1) {
        currentIndex++;
    updateFlashcard();
        }
    }

    // Hàm chuyển về flashcard trước
    function showPrevious() {
        if (currentIndex > 0) {
        currentIndex--;
    updateFlashcard();
        }
    }

    // Hàm thay đổi mặt của flashcard
    function toggleFlip() {
        document.getElementById("flashcard").classList.toggle("flipped");
    }

    // Khởi tạo flashcard đầu tiên
    updateFlashcard();
</script>
