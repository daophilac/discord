package com.peanut.discord.models;

public class User {
    private int userId;
    private String email;
    private String phone;
    private String password;
    private String userName;
    private String imageName;
    public int getUserId() {
        return userId;
    }
    public void setUserId(int userId) {
        this.userId = userId;
    }
    public String getEmail() {
        return email;
    }
    public void setEmail(String email) {
        this.email = email;
    }
    public String getPhone() {
        return phone;
    }
    public void setPhone(String phone) {
        this.phone = phone;
    }
    public String getPassword() {
        return password;
    }
    public void setPassword(String password) {
        this.password = password;
    }
    public String getUserName() {
        return userName;
    }
    public void setUserName(String userName) {
        this.userName = userName;
    }
    public String getImageName() {
        return imageName;
    }
    public void setImageName(String imageName) {
        this.imageName = imageName;
    }
    public User(){

    }
    public User(String email, String phone, String password, String userName){
        setEmail(email);
        setPhone(phone);
        setPassword(password);
        setUserName(userName);
    }
}
