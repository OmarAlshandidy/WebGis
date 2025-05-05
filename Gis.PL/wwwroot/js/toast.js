function showMessage(message, isError = false) {
    const toastElement = document.getElementById('messageToast');
    const toastBody = toastElement.querySelector('.toast-body');
    const toastHeader = toastElement.querySelector('.toast-header strong');

    toastHeader.textContent = isError ? 'Error' : 'Notification';
    toastBody.textContent = message;
    toastElement.classList.remove('bg-success', 'bg-danger');
    toastElement.classList.add(isError ? 'bg-danger' : 'bg-success');

    const toast = new bootstrap.Toast(toastElement, { delay: 5000 });
    toast.show();
}
