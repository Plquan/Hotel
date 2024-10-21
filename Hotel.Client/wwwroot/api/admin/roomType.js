function editRoomTypeId(id) {
    localStorage.setItem('editRoomTypeId', id);
    window.location.href = 'https://localhost:7060/Admin/RoomType/Edit';
}
function deleteRoomType(id) {
    const button = document.getElementById('confirmDelete');
    button.addEventListener('click', function () {
        axios.delete(`https://localhost:7197/api/RoomType/Delete/${id}`)
            .then(function (response) {
                console.log("Room deleted successfully:", response.data);

                location.reload();
            })
            .catch(function (error) {
                console.error("Error deleting roomtype:", error);
                alert("Lỗi khi xóa : " + error.response.data.message); 
            });
    });

}
function cancel() {
    window.location.href = 'https://localhost:7060/Admin/RoomType/';
}
function addRoomType() {
    const name = document.getElementById('name').value;
    const content = editorContent.getData();
    const capacity = document.getElementById('capacity').value;
    const bedType = document.getElementById('bedType').value;
    const view = document.getElementById('view').value;
    const size = document.getElementById('size').value;
    const price = document.getElementById('price').value;
    const base64Img = document.getElementById('thumb').src;
    const thumb = base64Img.replace(/^data:image\/[^;]+;base64,/, '');


    // Lấy danh sách tiện nghi
    const facility = tagify2.value;
    const listFacility = facility.map(facility => facility.value);

    // Lấy danh sách dịch vụ
    const service = tagify1.value;
    const listService = service.map(service => service.value);

    // Lấy danh sách ảnh
    const imgContainer = document.querySelector('#img_container');
    const images = imgContainer.querySelectorAll('img');
    const listImage = [];
    images.forEach(img => {
        const srcWithoutPrefix = img.src.replace(/^data:image\/[^;]+;base64,/, '');
        listImage.push(srcWithoutPrefix);
    });


    const Data = {
        Name: name,
        Content: content,
        Capacity: capacity,
        Price: price,
        View: view,
        BedType: bedType,
        Size: size,
        Thumb: thumb,
        RoomServices: listService,
        RoomFacilitys: listFacility,
        RoomImages: listImage
    };

    console.log(Data)

    //axios.post('https://localhost:7197/api/RoomType/Add',Data)
    //    .then(function (response) {
    //        console.log('Phòng đã được thêm thành công:', response.data);
          
    //    })
    //    .catch(function (error) {
    //        console.error('Lỗi khi thêm phòng:', error);
    //    });
}

function editRoomType() {
    const id = localStorage.getItem('editRoomTypeId');
    const name = document.getElementById('name').value;
    const content = CKEDITOR.instances.content.getData();
    const capacity = document.getElementById('capacity').value;
    const bedType = document.getElementById('bedType').value;
    const view = document.getElementById('view').value;
    const price = document.getElementById('price').value;
    const thumb = document.getElementById('thumb').src;
  
    //Lấy danh sách id dịch vụ đã chọn
    const listServices = [];
    const checkboxes = document.querySelectorAll('input[name="services"]:checked');
    checkboxes.forEach(checkbox => {
        listServices.push(
           checkbox.value
        ); 
    });

    // Lấy danh sách tag
    const facility = tagify.value;
    const listFacility = tags.map(facility => facility.value);

    // Lấy danh sách ảnh
    const imgContainer = document.querySelector('#img_container');
    const images = imgContainer.querySelectorAll('img'); 
    const listImage = [];
    images.forEach(img => {
        const srcWithoutPrefix = img.src.replace(/^data:image\/[^;]+;base64,/, '');
        listImage.push(srcWithoutPrefix); 
    });

    // Gán dữ liệu
    const Data = {
        Id: id,
        Name: name,
        Content: content,
        Capacity: capacity,
        Price: price,
        View: view,
        BedType: bedType,
        Thumb:thumb,
        Services: listServices,
        Facilitys: listFacility,
        Images: listImage
    };
    console.log(Data)
    //Đẩy dữ liệu
    axios.put('https://localhost:7197/api/RoomType/Update', Data)
        .then(function (response) {
            console.log('Phòng đã được cập nhật thành công:', response.data);
            //window.location.href = 'https://localhost:7060/Admin/RoomType';
                  localStorage.removeItem('editRoomTypeId');
        })
        .catch(function (error) {
            console.error('Lỗi khi cập nhật phòng:', error);
        });
}


