package com.daophilac.discord.models;
enum Gender{
    Male, Female
}
public class User {
    private int UserID;
    private String Email;
    private String Password;
    private String UserName;
    private String FistName;
    private String LastName;
    private Gender Gender;
    private String Image;

    public int getUserID() {
        return UserID;
    }

    public void setUserID(int userID) {
        this.UserID = userID;
    }

    public String getEmail() {
        return Email;
    }

    public void setEmail(String email) {
        this.Email = email;
    }

    public String getPassword() {
        return Password;
    }

    public void setPassword(String password) {
        this.Password = password;
    }

    public String getUserName() {
        return UserName;
    }

    public void setUserName(String userName) {
        this.UserName = userName;
    }

    public String getFirstName() {
        return FistName;
    }

    public void setFirstName(String firstName) {
        FistName = firstName;
    }

    public String getLastName() {
        return LastName;
    }

    public void setLastName(String lastName) {
        this.LastName = lastName;
    }

    public com.daophilac.discord.models.Gender getGender() {
        return Gender;
    }

    public void setGender(com.daophilac.discord.models.Gender gender) {
        this.Gender = gender;
    }

    public String getImage() {
        return Image;
    }

    public void setImage(String image) {
        this.Image = image;
    }
}
